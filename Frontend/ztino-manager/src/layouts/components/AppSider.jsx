import React, { useState } from 'react';
import { Layout, Drawer } from 'antd';
import { useLocation, useNavigate } from 'react-router-dom';
import { ChevronDownIcon, XMarkIcon } from "@heroicons/react/24/outline";
import { getMenuItems } from '../config/menuItems';
import { useAuth } from '../../contexts/AuthContext';

const { Sider } = Layout;

const AppSider = ({ collapsed, isMobile, isOpen, setIsOpen }) => {
  const navigate = useNavigate();
  const location = useLocation();
  const { user } = useAuth();
  
  const [openSubmenus, setOpenSubmenus] = useState({});

  const userRoles = user?.roles || [];
  const menuItems = getMenuItems(userRoles);

  const toggleSubmenu = (key) => {
    setOpenSubmenus((prev) => ({
      ...prev,
      [key]: !prev[key],
    }));
  };

  const isActiveLink = (path) => location.pathname === path;
  const isParentActive = (submenu) => submenu.some(sub => sub.key === location.pathname);

  const SidebarContent = () => (
    <div className="flex flex-col h-full bg-white text-gray-600 font-sans">
      
      <div className="h-16 flex items-center px-6 border-b border-gray-100 flex-shrink-0">
        <div className="w-8 h-8 bg-indigo-600 rounded-lg flex items-center justify-center text-white font-bold mr-3 flex-shrink-0 text-base">
          Z
        </div>
        {(!collapsed || isMobile) && (
          <span className="text-xl font-bold text-gray-800 tracking-tight">
            Z-Tino Manager
          </span>
        )}
        {isMobile && (
           <button onClick={() => setIsOpen(false)} className="ml-auto p-1 rounded hover:bg-gray-100 transition-colors">
              <XMarkIcon className="w-6 h-6 text-gray-500" />
           </button>
        )}
      </div>

      <nav className="flex-1 overflow-y-auto py-4 px-3 space-y-1 custom-scrollbar">
        {menuItems.map((item, index) => {
            if (item.type === 'divider') {
                return (!collapsed || isMobile) ? <div key={index} className="h-px bg-gray-100 my-4 mx-2" /> : null;
            }

            const Icon = item.icon;
            const hasSubmenu = item.submenu && item.submenu.length > 0;
            const isActive = isActiveLink(item.key);
            const isSubOpen = openSubmenus[item.key] || isParentActive(item.submenu || []);
            const isComingSoon = item.comingSoon;
            
            const itemClasses = `
                group flex items-center w-full p-3 rounded-xl transition-all duration-200 font-medium text-sm
                ${isComingSoon 
                  ? "opacity-50 cursor-not-allowed text-gray-400" 
                  : isActive && !hasSubmenu 
                    ? "bg-indigo-600 text-white shadow-md shadow-indigo-200"
                    : "text-gray-500 hover:bg-indigo-50 hover:text-indigo-600"
                }
                ${collapsed && !isMobile ? "justify-center px-2" : "justify-start"}
            `;

            return (
                <div key={item.key}>
                    <button 
                        disabled={isComingSoon}
                        onClick={() => {
                            if (isComingSoon) return;
                            if (hasSubmenu) {
                                if (collapsed && !isMobile) return;
                                toggleSubmenu(item.key);
                            } else {
                                navigate(item.key);
                                if (isMobile) setIsOpen(false);
                            }
                        }}
                        className={itemClasses}
                        title={collapsed ? item.label : ""}
                    >
                        <Icon className={`w-6 h-6 flex-shrink-0 ${collapsed && !isMobile ? "" : "mr-3"}`} />

                        {(!collapsed || isMobile) && (
                            <>
                                <span className="flex-1 text-left whitespace-nowrap">{item.label}</span>
                                
                                {isComingSoon && (
                                    <span className="text-[10px] bg-gray-100 text-gray-500 px-2 py-0.5 rounded-full ml-2 border border-gray-200">Soon</span>
                                )}

                                {hasSubmenu && !isComingSoon && (
                                    <ChevronDownIcon 
                                        className={`w-4 h-4 transition-transform duration-200 ${isSubOpen ? "rotate-180" : ""}`} 
                                    />
                                )}
                            </>
                        )}
                    </button>

                    {hasSubmenu && !isComingSoon && (!collapsed || isMobile) && isSubOpen && (
                        <div className="mt-1 ml-3 pl-3 border-l border-gray-100 space-y-1">
                            {item.submenu.map(subItem => (
                                <button
                                    key={subItem.key}
                                    onClick={() => {
                                        navigate(subItem.key);
                                        if (isMobile) setIsOpen(false);
                                    }}
                                    className={`
                                        items-center w-full px-3 py-2 rounded-lg text-sm transition-colors block text-left
                                        ${isActiveLink(subItem.key)
                                            ? "text-indigo-600 bg-indigo-50 font-medium"
                                            : "text-gray-500 hover:text-gray-900 hover:bg-gray-50"
                                        }
                                    `}
                                >
                                    <span className="w-1.5 h-1.5 rounded-full bg-current mr-2 opacity-40"></span>
                                    {subItem.label}
                                </button>
                            ))}
                        </div>
                    )}
                </div>
            );
        })}
      </nav>
      
      <div className="p-4 border-t border-gray-100 bg-gray-50/30">
          <div className={`flex items-center ${collapsed && !isMobile ? "justify-center" : "gap-3"}`}>
              <img 
                  src={`https://ui-avatars.com/api/?name=${user?.name || 'Manager'}&background=6366f1&color=fff&bold=true`} 
                  alt="User" 
                  className="w-9 h-9 rounded-full ring-2 ring-white shadow-sm flex-shrink-0 object-cover"
              />
              {(!collapsed || isMobile) && (
                  <div className="overflow-hidden">
                      <p className="text-sm font-semibold text-gray-800 truncate leading-tight">{user?.name || 'Manager'}</p>
                      <p className="text-[11px] text-gray-400 truncate mt-0.5">{user?.email || 'manager@ztino.com'}</p>
                  </div>
              )}
          </div>
      </div>
    </div>
  );

  if (isMobile) {
    return (
      <Drawer
        placement="left"
        onClose={() => setIsOpen(false)}
        open={isOpen}
        width={280}
        styles={{ body: { padding: 0 } }}
        closable={false}
      >
        <SidebarContent />
      </Drawer>
    );
  }

  return (
    <Sider
      trigger={null}
      collapsible
      collapsed={collapsed}
      width={260}
      theme="light"
      className="!fixed left-0 top-0 bottom-0 z-50 h-screen border-r border-gray-100 shadow-[2px_0_8px_rgba(0,0,0,0.01)]"
    >
      <SidebarContent />
    </Sider>
  );
};

export default AppSider;