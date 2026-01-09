import React, { memo, useMemo } from 'react';
import { Dropdown, Button, Tag } from 'antd';
import { EditOutlined, DownOutlined } from '@ant-design/icons';
import { STATUS_TRANSITIONS, FINAL_STATUSES, STATUS_COLOR } from '../constants';

const UpdateStatusButton = memo(({ currentStatus, onStatusSelect, disabled }) => {
    const isFinalStatus = useMemo(() => {
        return FINAL_STATUSES.includes(currentStatus);
    }, [currentStatus]);

    const allowedStatuses = useMemo(() => {
        return STATUS_TRANSITIONS[currentStatus] || [];
    }, [currentStatus]);

    const menuItems = useMemo(() => {
        return allowedStatuses.map((status) => ({
            key: status,
            label: (
                <div className="flex items-center gap-2 py-1">
                    <Tag color={STATUS_COLOR[status]} className="m-0">
                        {status}
                    </Tag>
                </div>
            )
        }));
    }, [allowedStatuses]);

    const handleMenuClick = useMemo(() => {
        return ({ key }) => {
            onStatusSelect(key);
        };
    }, [onStatusSelect]);

    if (isFinalStatus || allowedStatuses.length === 0) {
        return null;
    }

    return (
        <Dropdown
            menu={{
                items: menuItems,
                onClick: handleMenuClick
            }}
            trigger={['click']}
            disabled={disabled}
        >
            <Button 
                type="primary"
                icon={<EditOutlined />}
                className="bg-indigo-600 hover:bg-indigo-700"
            >
                Update Status <DownOutlined className="ml-1" />
            </Button>
        </Dropdown>
    );
});

UpdateStatusButton.displayName = 'UpdateStatusButton';

export default UpdateStatusButton;