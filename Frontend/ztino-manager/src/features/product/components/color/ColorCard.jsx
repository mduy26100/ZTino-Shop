import React from 'react';
import { Card, Typography, Button, Tooltip, theme } from 'antd';
import { PencilSquareIcon, TrashIcon } from '@heroicons/react/24/outline';

const { Text } = Typography;

const ColorCard = ({ item, onEdit, onDelete }) => {
    const { token } = theme.useToken();

    const colorStyle = {
        backgroundColor: item.hex || item.name?.toLowerCase(),
        border: `1px solid ${token.colorBorderSecondary}`,
    };

    return (
        <Card
            hoverable
            className="h-full shadow-sm hover:shadow-md transition-all duration-300 rounded-2xl overflow-hidden"
            styles={{ 
                body: { padding: '24px' },
                actions: { display: 'flex', alignItems: 'center' } 
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
            <div className="flex flex-col items-center gap-4">
                <div 
                    className="w-16 h-16 rounded-full shadow-inner ring-4 ring-gray-50 transition-transform hover:scale-105"
                    style={colorStyle}
                />
                
                <div className="text-center w-full">
                    <Text strong className="text-base block truncate px-2 text-slate-800">
                        {item.name}
                    </Text>
                    <Text type="secondary" className="text-[11px] font-mono uppercase tracking-wider text-slate-400">
                        {item.hex || `ID-${item.id}`}
                    </Text>
                </div>
            </div>
        </Card>
    );
};

export default React.memo(ColorCard);