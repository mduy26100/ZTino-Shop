import React, { useCallback, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { CategorySection, EditorialSection, HeroSection, NewArrivalsSection, ServicesSection } from '../../../features';

const HomePage = () => {
    const navigate = useNavigate();

    const MOCK_DATA = useMemo(() => ({
        hero: {
            title: "ZTino Menswear",
            subtitle: "Timeless style for every season. Premium menswear crafted for the modern man.",
            image: "https://images.unsplash.com/photo-1516826957135-700dedea698c?q=80&w=2070&auto=format&fit=crop", 
            btnText: "Shop New Arrivals"
        },
        categories: [
            { 
                id: 1, 
                name: "Casual Shirts", 
                slug: "casual-shirts",
                isActive: true,
                parentId: null,
                image: "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=600&auto=format&fit=crop" 
            },
            { 
                id: 2, 
                name: "Essential Tees", 
                slug: "essential-tees",
                isActive: true,
                parentId: null,
                image: "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=600&auto=format&fit=crop" 
            },
            { 
                id: 3, 
                name: "Denim", 
                slug: "denim",
                isActive: true,
                parentId: null,
                image: "https://images.unsplash.com/photo-1542272454315-4c01d7abdf4a?w=600&auto=format&fit=crop" 
            },
            { 
                id: 4, 
                name: "Outerwear", 
                slug: "outerwear",
                isActive: true,
                parentId: null,
                image: "https://images.unsplash.com/photo-1512436991641-6745cdb1723f?q=80&w=600&auto=format&fit=crop" 
            },
            { 
                id: 5, 
                name: "Sneakers", 
                slug: "sneakers",
                isActive: true,
                parentId: null,
                image: "https://images.unsplash.com/photo-1552346154-21d32810aba3?w=600&auto=format&fit=crop" 
            },
            { 
                id: 6, 
                name: "Formal Wear", 
                slug: "formal-wear",
                isActive: true,
                parentId: null,
                image: "https://images.unsplash.com/photo-1523275335684-37898b6baf30?q=80&w=600&auto=format&fit=crop" 
            },
            { 
                id: 7, 
                name: "Accessories", 
                slug: "accessories",
                isActive: true,
                parentId: null,
                image: "https://images.unsplash.com/photo-1503342217505-b0a15ec3261c?q=80&w=600&auto=format&fit=crop" 
            },
        ],
        products: [
            {
                id: 101,
                categoryId: 1,
                name: "The Oxford Shirt",
                slug: "the-oxford-shirt",
                basePrice: 550000,
                description: "Classic oxford shirt for formal and casual wear.",
                mainImageUrl: "https://images.unsplash.com/photo-1598033129183-c4f50c736f10?w=800&auto=format&fit=crop",
                isActive: true,
                createdAt: "2024-01-01T00:00:00Z",
                tag: "Bestseller"
            },
            {
                id: 102,
                categoryId: 3,
                name: "Pleated Chino Trousers",
                slug: "pleated-chino-trousers",
                basePrice: 750000,
                description: "Comfortable pleated chinos.",
                mainImageUrl: "https://images.unsplash.com/photo-1624378439575-d8705ad7ae80?w=800&auto=format&fit=crop",
                isActive: true,
                createdAt: "2024-01-02T00:00:00Z",
                tag: "New"
            },
            {
                id: 103,
                categoryId: 5,
                name: "Leather Low-Top Sneakers",
                slug: "leather-low-top-sneakers",
                basePrice: 1850000,
                description: "Premium leather sneakers.",
                mainImageUrl: "https://images.unsplash.com/photo-1600185365483-26d7a4cc7519?q=80&w=800&auto=format&fit=crop",
                isActive: true,
                createdAt: "2024-01-03T00:00:00Z",
                tag: "New"
            },
            {
                id: 104,
                categoryId: 4,
                name: "Relaxed Fit Hoodie",
                slug: "relaxed-fit-hoodie",
                basePrice: 650000,
                description: "Cozy relaxed fit hoodie.",
                mainImageUrl: "https://images.unsplash.com/photo-1556821840-3a63f95609a7?w=800&auto=format&fit=crop",
                isActive: true,
                createdAt: "2024-01-04T00:00:00Z",
                tag: "Trending"
            }
        ]
    }), []);

    const handleNavigate = useCallback((path) => {
        navigate(path);
    }, [navigate]);

    const handleCategoryClick = useCallback((id) => {
        navigate(`/products?category=${id}`);
    }, [navigate]);

    const handleProductClick = useCallback((id) => {
        navigate(`/products/${id}`);
    }, [navigate]);

    return (
        <div className="bg-white font-sans text-slate-800">
            <HeroSection 
                data={MOCK_DATA.hero} 
                onCtaClick={() => handleNavigate('/products')} 
            />

            <CategorySection 
                categories={MOCK_DATA.categories} 
                onCategoryClick={handleCategoryClick} 
            />

            <NewArrivalsSection 
                products={MOCK_DATA.products} 
                onViewAll={() => handleNavigate('/products')}
                onProductClick={handleProductClick}
            />

            <EditorialSection />

            <ServicesSection />
        </div>
    );
};

export default HomePage;