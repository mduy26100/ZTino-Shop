import React, { memo } from 'react';
import { Row, Col, Spin, Empty, Typography } from 'antd';
import ProductCard from './ProductCard';
import ProductFilter from './ProductFilter';

const { Title } = Typography;

const ProductGrid = ({ 
    products, 
    isLoading, 
    title = "All Products" 
}) => {
    return (
        <div className="w-full">
            <div className="mb-8 lg:mb-12 text-center">
                <Title level={2} className="!font-serif text-slate-900 !text-3xl lg:!text-4xl capitalize !mb-2">
                    {title}
                </Title>
                <div className="w-12 h-0.5 bg-slate-900 mx-auto opacity-50"></div>
                {!isLoading && (
                    <span className="text-gray-500 text-sm mt-3 block uppercase tracking-widest">
                        {products?.length || 0} Products Found
                    </span>
                )}
            </div>

            <Row gutter={[48, 0]}>
                <Col xs={0} lg={6} xl={5} className="hidden lg:block">
                    <div className="sticky top-24">
                        <ProductFilter />
                    </div>
                </Col>

                <Col xs={24} lg={18} xl={19}>
                    {isLoading ? (
                        <div className="flex justify-center items-center h-[50vh] w-full">
                            <Spin size="large" />
                        </div>
                    ) : (!products || products.length === 0) ? (
                        <div className="py-20 flex flex-col items-center justify-center bg-gray-50">
                            <Empty 
                                image={Empty.PRESENTED_IMAGE_SIMPLE} 
                                description={<span className="text-gray-500 font-light">No products found in this collection.</span>} 
                            />
                        </div>
                    ) : (
                        <Row gutter={[24, 48]}>
                            {products.map((product) => (
                                <Col key={product.id} xs={12} sm={12} md={8} lg={8} xl={6}>
                                    <ProductCard product={product} />
                                </Col>
                            ))}
                        </Row>
                    )}
                </Col>
            </Row>
        </div>
    );
};

export default memo(ProductGrid);