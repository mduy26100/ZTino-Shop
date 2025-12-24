import { Outlet } from "react-router-dom";

const MainLayout = () => {
    return (
        <div className="dashboard-layout">
            <header style={{ padding: '1rem', borderBottom: '1px solid #ccc' }}>
                <h1>Admin Main Layout</h1>
            </header>
            
            <main style={{ padding: '2rem' }}>
                <Outlet /> 
            </main>
        </div>
    );
};

export default MainLayout;