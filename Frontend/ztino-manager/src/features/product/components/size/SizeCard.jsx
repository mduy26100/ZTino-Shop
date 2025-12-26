import React from 'react';
import { Card, Typography, Button, Tooltip, theme } from 'antd';
import { PencilSquareIcon, TrashIcon } from '@heroicons/react/24/outline';

const { Text } = Typography;

const SizeCard = ({ item, onEdit, onDelete }) => {
    const { token } = theme.useToken();

    return (
        <Card
            hoverable
            className="h-full shadow-sm hover:shadow-md transition-all duration-300 rounded-2xl overflow-hidden group"
            styles={{ 
                body: { padding: '24px', textAlign: 'center' },
                actions: { display: 'flex', alignItems: 'center', backgroundColor: '#fafafa' } 
            }}
            actions={[
                <div className="flex justify-center items-center w-full">
                    <Tooltip title="Edit">
                        <Button
                            type="text"
                            size="large"
                            icon={<PencilSquareIcon className="w-5 h-5 text-indigo-600" />}
                            className="flex items-center justify-center hover:bg-indigo-50 border-none shadow-none"
                            onClick={() => onEdit?.(item)}
                        />
                    </Tooltip>
                </div>,
                <div className="flex justify-center items-center w-full border-l border-gray-100">
                    <Tooltip title="Delete">
                        <Button
                            type="text"
                            size="large"
                            danger
                            icon={<TrashIcon className="w-5 h-5" />}
                            className="flex items-center justify-center hover:bg-rose-50 border-none shadow-none"
                            onClick={() => onDelete?.(item)}
                        />
                    </Tooltip>
                </div>
            ]}
        >
            <div className="flex flex-col items-center justify-center gap-2 py-2">
                <div 
                    className="w-16 h-16 rounded-2xl bg-slate-50 border border-slate-100 flex items-center justify-center mb-2 group-hover:scale-110 group-hover:bg-indigo-50 group-hover:border-indigo-100 transition-all duration-300"
                >
                    <Text strong className="text-2xl text-slate-700 group-hover:text-indigo-600">
                        {item.name}
                    </Text>
                </div>
                
                <Text type="secondary" className="text-xs font-mono uppercase tracking-wider text-slate-400">
                    ID: {item.id}
                </Text>
            </div>
        </Card>
    );
};

export default React.memo(SizeCard);