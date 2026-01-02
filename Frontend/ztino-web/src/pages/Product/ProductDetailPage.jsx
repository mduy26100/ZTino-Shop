import React, { useState, useMemo } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import { Spin, Breadcrumb, Result, Button } from 'antd';
import { HomeOutlined } from '@ant-design/icons';
import { ProductGallery, ProductInfo, useGetProductDetailBySlug } from '../../features';

const ProductDetailPage = () => {
    const { slug } = useParams(); 
    const navigate = useNavigate();
    const location = useLocation();
    
    const [selectedColorId, setSelectedColorId] = useState(null);

    const productId = location.state?.id || slug;

    const { data: productData, isLoading, error } = useGetProductDetailBySlug(productId);

    const product = useMemo(() => {
        if (!productData) return null;
        return productData.data || productData;
    }, [productData]);

    const displayImages = useMemo(() => {
        if (!product) return [];
        let imgs = [];

        if (selectedColorId) {
            const activeGroup = product.variantGroups?.find(g => g.colorId === selectedColorId);
            if (activeGroup?.images) {
                imgs = [...activeGroup.images];
            }
            
            if (imgs.length === 0 && product.mainImageUrl) {
                imgs.push({ id: 'main', imageUrl: product.mainImageUrl, isMain: true });
            }
        } else {
            if (product.mainImageUrl) {
                imgs.push({ id: 'main', imageUrl: product.mainImageUrl, isMain: true });
            }
            if (product.variantGroups) {
                product.variantGroups.forEach(group => {
                    if (group.images) {
                        imgs.push(...group.images);
                    }
                });
            }
        }

        const uniqueImgs = Array.from(new Set(imgs.map(a => a.imageUrl)))
            .map(url => imgs.find(a => a.imageUrl === url));

        return uniqueImgs;
    }, [product, selectedColorId]);

    if (isLoading) {
        return (
            <div className="h-screen flex items-center justify-center">
                <Spin size="large" />
            </div>
        );
    }

    if (error || !product) {
        return (
            <div className="h-screen flex items-center justify-center">
                <Result
                    status="404"
                    title="Product Not Found"
                    subTitle="Sorry, the product you visited does not exist."
                    extra={<Button type="primary" onClick={() => navigate('/products')}>Back to Shop</Button>}
                />
            </div>
        );
    }

    return (
        <div className="bg-white min-h-screen pb-20">
            <div className="max-w-[1440px] mx-auto px-6 py-6">
                <Breadcrumb 
                    items={[
                        { 
                            title: <span className="cursor-pointer hover:text-indigo-600 transition-colors"><HomeOutlined /></span>, 
                            onClick: () => navigate('/') 
                        },
                        { 
                            title: product.category?.name ? (
                                <span className="cursor-pointer hover:text-indigo-600 transition-colors">
                                    {product.category.name}
                                </span>
                            ) : null, 
                            onClick: () => product.category?.slug && navigate(`/products/${product.category.slug}`) 
                        },
                        { 
                            title: <span className="font-medium text-slate-900">{product.name}</span> 
                        }
                    ]}
                />
            </div>

            <div className="max-w-[1440px] mx-auto px-6 grid grid-cols-1 lg:grid-cols-2 gap-12 lg:gap-20">
                <div>
                    <ProductGallery images={displayImages} />
                </div>

                <div>
                    <ProductInfo 
                        product={product} 
                        selectedColorId={selectedColorId}
                        setSelectedColorId={setSelectedColorId}
                    />
                </div>
            </div>
        </div>
    );
};

export default ProductDetailPage;