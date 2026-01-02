import React, { useMemo } from 'react';
import { useParams } from 'react-router-dom';
import { ProductGrid, useGetCategories, useGetProductsByCategoryId } from '../../features';

const ProductListingPage = () => {
    const { slug } = useParams();
    const { data: categories } = useGetCategories();

    const activeCategory = useMemo(() => {
        if (!slug || !categories?.length) return null;

        const findInTree = (nodes) => {
            for (const node of nodes) {
                if (node.slug === slug) return node;
                if (node.children?.length > 0) {
                    const found = findInTree(node.children);
                    if (found) return found;
                }
            }
            return null;
        };

        return findInTree(categories);
    }, [categories, slug]);

    const { 
        data: products, 
        isLoading 
    } = useGetProductsByCategoryId(activeCategory?.id || null);

    const pageTitle = activeCategory ? activeCategory.name : 'The Collection';

    return (
        <div className="bg-white min-h-screen">
            <div className="max-w-[1600px] mx-auto px-4 md:px-8 py-12">
                <ProductGrid 
                    products={products} 
                    isLoading={isLoading} 
                    title={pageTitle}
                />
            </div>
        </div>
    );
};

export default ProductListingPage;