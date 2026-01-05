import React from 'react';
import { Layout, Button, Badge, Dropdown, theme, Avatar } from 'antd';
import { 
    Bars3CenterLeftIcon,
    BellIcon,
    ChevronDownIcon 
} from '@heroicons/react/24/outline';
import { useAuth } from '../../contexts';
import { UserOutlined, LogoutOutlined } from '@ant-design/icons';

const { Header } = Layout;

const AppHeader = ({ collapsed, setCollapsed, isMobile, setIsOpen }) => {
  const { logout, user } = useAuth();
  const { token } = theme.useToken();

  const userMenu = [
    {
      key: 'profile',
      label: <span className="font-medium">My Profile</span>,
      icon: <UserOutlined />,
    },
    { type: 'divider' },
    {
      key: 'logout',
      label: <span className="text-red-500">Logout</span>,
      icon: <LogoutOutlined className="text-red-500" />,
      onClick: logout,
    },
  ];

  return (
    <Header 
        className="sticky top-0 z-40 flex items-center justify-between px-6 bg-white/80 backdrop-blur-md border-b border-gray-100"
        style={{ height: 64, paddingInline: 24 }}
    >
      <div className="flex items-center gap-4">
          <button 
            onClick={() => isMobile ? setIsOpen(true) : setCollapsed(!collapsed)}
            className="p-2 rounded-lg text-gray-500 hover:bg-gray-100 transition-colors"
          >
             <Bars3CenterLeftIcon className="w-6 h-6" />
          </button>
          
          <h2 className="text-lg font-semibold text-gray-800 hidden md:block">
            Dashboard
          </h2>
      </div>

      <div className="flex items-center gap-3">
          <button className="relative p-2 rounded-full text-gray-500 hover:bg-gray-100 transition-colors">
             <BellIcon className="w-6 h-6" />
             <span className="absolute top-2 right-2 w-2 h-2 bg-red-500 rounded-full border border-white"></span>
          </button>

          <div className="h-8 w-px bg-gray-200 mx-2 hidden sm:block"></div>

          <Dropdown menu={{ items: userMenu }} trigger={['click']}>
              <button className="flex items-center gap-2 hover:bg-gray-50 p-1 pr-2 rounded-full transition-colors">
                  <Avatar 
                    className="bg-gradient-to-r from-blue-500 to-indigo-600 border border-blue-200" 
                    icon={<UserOutlined />} 
                  />    
                  <span className="text-sm font-medium text-gray-700 hidden sm:block">
                    {user?.name || 'Admin'}
                  </span>
                  <ChevronDownIcon className="w-4 h-4 text-gray-400 hidden sm:block" />
              </button>
          </Dropdown>
      </div>
    </Header>
  );
};

export default AppHeader;