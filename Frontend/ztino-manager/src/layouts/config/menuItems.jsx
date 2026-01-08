import {
  Squares2X2Icon,
  CubeIcon,
  ShoppingCartIcon,
  UsersIcon,
  Cog6ToothIcon,
  ChartBarIcon,
} from "@heroicons/react/24/outline";

export const MENU_ITEMS = [
  {
    key: '/dashboard',
    label: 'Dashboard',
    icon: Squares2X2Icon,
    roles: ['Manager'],
  },
  {
    key: 'products',
    label: 'Products',
    icon: CubeIcon,
    roles: ['Manager'],
    submenu: [
        { key: '/products', label: 'Products' },
        { key: '/categories', label: 'Categories' },
        { key: '/colors', label: 'Colors' },
        { key: '/sizes', label: 'Sizes' },
    ]
  },
  {
    key: '/orders',
    label: 'Orders',
    icon: ShoppingCartIcon,
    roles: ['Manager'],
  },
  {
    type: 'divider',
  },
  {
    key: '/users',
    label: 'Customers',
    icon: UsersIcon,
    roles: ['Manager'],
    comingSoon: true,
  },
  {
    key: '/reports',
    label: 'Reports',
    icon: ChartBarIcon,
    roles: ['Manager'],
    comingSoon: true,
  },
  {
    key: '/settings',
    label: 'Settings',
    icon: Cog6ToothIcon,
    roles: ['Manager'],
    comingSoon: true,
  },
];

export const getMenuItems = (userRoles = []) => {
    return MENU_ITEMS.filter(item => {
        if (item.type === 'divider') return true;
        if (!item.roles) return true; 
        return item.roles.some(role => userRoles.includes(role));
    });
};