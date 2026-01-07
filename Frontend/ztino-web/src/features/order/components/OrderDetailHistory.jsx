import React, { memo, useMemo } from 'react';
import { Card, Timeline, Typography, Tag } from 'antd';
import { 
    ClockCircleOutlined,
    CheckCircleOutlined,
    CarOutlined,
    GiftOutlined,
    CloseCircleOutlined,
    RollbackOutlined
} from '@ant-design/icons';
import dayjs from 'dayjs';

const { Title, Text } = Typography;

const STATUS_CONFIG = {
    Pending: { color: 'orange', icon: ClockCircleOutlined },
    Confirmed: { color: 'blue', icon: CheckCircleOutlined },
    Shipping: { color: 'cyan', icon: CarOutlined },
    Delivered: { color: 'green', icon: GiftOutlined },
    Cancelled: { color: 'red', icon: CloseCircleOutlined },
    Returned: { color: 'purple', icon: RollbackOutlined }
};

const formatDateTime = (dateString) => {
    return dayjs(dateString).format('DD/MM/YYYY HH:mm:ss');
};

const OrderDetailHistory = memo(({ histories = [] }) => {
    const timelineItems = useMemo(() => {
        const sortedHistories = [...histories].sort(
            (a, b) => new Date(b.createdAt) - new Date(a.createdAt)
        );

        return sortedHistories.map((history, index) => {
            const config = STATUS_CONFIG[history.status] || { color: 'gray', icon: ClockCircleOutlined };
            const IconComponent = config.icon;
            const isLatest = index === 0;

            return {
                key: `${history.status}-${history.createdAt}`,
                color: config.color,
                dot: <IconComponent className={`text-${config.color}-500`} />,
                children: (
                    <div className={`pb-2 ${isLatest ? 'animate-pulse-once' : ''}`}>
                        <div className="flex items-center gap-2 mb-1 flex-wrap">
                            <Tag color={config.color} className="m-0">
                                {history.status}
                            </Tag>
                            <Text className="text-gray-400 text-xs">
                                {formatDateTime(history.createdAt)}
                            </Text>
                        </div>
                        {history.note && (
                            <Text className="text-gray-600 text-sm">
                                {history.note}
                            </Text>
                        )}
                    </div>
                )
            };
        });
    }, [histories]);

    if (!histories || histories.length === 0) {
        return null;
    }

    return (
        <Card className="shadow-sm rounded-xl mb-6">
            <Title level={5} className="!mb-4 text-gray-800">
                Order History
            </Title>
            <Timeline items={timelineItems} />
        </Card>
    );
});

OrderDetailHistory.displayName = 'OrderDetailHistory';

export default OrderDetailHistory;
