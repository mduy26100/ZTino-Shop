import { useLocation } from 'react-router-dom';
import { message } from 'antd';
import { useEffect, useRef } from 'react';

const Dashboard = () => {
    const location = useLocation();
    const [messageApi, contextHolder] = message.useMessage();
    
    const hasShownToast = useRef(false);

    useEffect(() => {
        if (location.state?.showSuccessLogin && !hasShownToast.current) {
            
            messageApi.success('Login successful!');
            
            hasShownToast.current = true;

            window.history.replaceState({}, document.title);
        }
    }, [location.state, messageApi]);

    return (
        <div>
            {contextHolder}
            <div style={{ padding: 20 }}>
                <h1>Dashboard Page</h1>
                <p>Welcome to dashboard</p>
            </div>
        </div>
    )
}

export default Dashboard;