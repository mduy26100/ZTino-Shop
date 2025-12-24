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
            styles={{ body: { padding: '20px' } }}
            actions={[
                <Tooltip title="Edit" key="edit">
                    <Button
                        type="text"
                        size="small"
                        icon={<PencilSquareIcon className="w-5 h-5 text-indigo-600" />}
                        className="flex items-center justify-center w-full hover:bg-indigo-50"
                        onClick={() => onEdit?.(item)}
                    />
                </Tooltip>,
                <Tooltip title="Delete" key="delete">
                    <Button
                        type="text"
                        size="small"
                        danger
                        icon={<TrashIcon className="w-5 h-5" />}
                        className="flex items-center justify-center w-full hover:bg-rose-50"
                        onClick={() => onDelete?.(item)}
                    />
                </Tooltip>
            ]}
        >
            <div className="flex flex-col items-center gap-4">
                <div 
                    className="w-16 h-16 rounded-full shadow-inner ring-4 ring-gray-50"
                    style={colorStyle}
                />
                
                <div className="text-center w-full">
                    <Text strong className="text-base block truncate px-2">
                        {item.name}
                    </Text>
                    <Text type="secondary" className="text-[11px] font-mono uppercase tracking-wider">
                        {item.hex || `ID-${item.id}`}
                    </Text>
                </div>
            </div>
        </Card>
    );
};

export default React.memo(ColorCard);