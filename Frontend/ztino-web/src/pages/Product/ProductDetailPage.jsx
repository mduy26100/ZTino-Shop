import React, { useState, useMemo, useCallback } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import { Spin, Breadcrumb, Result, Button } from 'antd';
import { HomeOutlined } from '@ant-design/icons';
import { ProductGallery, ProductInfo, useGetProductDetailBySlug } from '../../features';

const ProductDetailPage = () => {
    const { slug } = useParams(); 
    const navigate = useNavigate();
    const location = useLocation();
    
    const [selectedProductColorId, setSelectedProductColorId] = useState(null);

    const productId = location.state?.id || slug;

    const { data: productData, isLoading, error } = useGetProductDetailBySlug(productId);

    const product = useMemo(() => {
        if (!productData) return null;
        return productData.data || productData;
    }, [productData]);

    const sortImages = useCallback((images) => {
        if (!images?.length) return [];
        return [...images].sort((a, b) => {
            if (a.isMain !== b.isMain) return a.isMain ? -1 : 1;
            return (a.displayOrder || 0) - (b.displayOrder || 0);
        });
    }, []);

    const imageToColorMap = useMemo(() => {
        const map = new Map();
        if (!product?.productColors) return map;
        
        product.productColors.forEach(pc => {
            pc.images?.forEach(img => {
                map.set(img.id, pc.id);
            });
        });
        return map;
    }, [product?.productColors]);

    const displayImages = useMemo(() => {
        if (!product) return [];

        if (selectedProductColorId) {
            const activeProductColor = product.productColors?.find(
                pc => pc.id === selectedProductColorId
            );
            
            if (activeProductColor?.images?.length > 0) {
                return sortImages(activeProductColor.images);
            }
            
            if (product.mainImageUrl) {
                return [{ id: 'main', imageUrl: product.mainImageUrl, isMain: true }];
            }
            return [];
        }

        const images = [];
        
        if (product.mainImageUrl) {
            images.push({ id: 'main', imageUrl: product.mainImageUrl, isMain: true, displayOrder: -1 });
        }

        if (product.productColors) {
            product.productColors.forEach(pc => {
                if (pc.images?.length > 0) {
                    pc.images.forEach(img => {
                        if (img.imageUrl !== product.mainImageUrl) {
                            images.push({ ...img, productColorId: pc.id });
                        }
                    });
                }
            });
        }

        return sortImages(images);
    }, [product, selectedProductColorId, sortImages]);

    const handleImageClick = useCallback((imageId) => {
        if (imageId === 'main') return;
        
        const colorId = imageToColorMap.get(imageId);
        if (colorId) {
            setSelectedProductColorId(colorId);
        }
    }, [imageToColorMap]);

    const handleResetSelection = useCallback(() => {
        setSelectedProductColorId(null);
    }, []);

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
                    <ProductGallery 
                        images={displayImages} 
                        onImageClick={handleImageClick}
                    />
                </div>

                <div>
                    <ProductInfo 
                        product={product} 
                        selectedProductColorId={selectedProductColorId}
                        setSelectedProductColorId={setSelectedProductColorId}
                        onResetSelection={handleResetSelection}
                    />
                </div>
            </div>
        </div>
    );
};

export default ProductDetailPage;