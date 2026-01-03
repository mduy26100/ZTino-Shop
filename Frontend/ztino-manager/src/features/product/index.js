//components
//Category
export {default as CategoryTable} from "./components/category/CategoryTable";
export {default as UpsertCategoryModal} from "./components/category/UpsertCategoryModal";

//Color
export {default as ColorCard} from "./components/color/ColorCard";
export {default as UpsertColorModal} from "./components/color/UpsertColorModal";

//Size
export {default as SizeCard} from "./components/size/SizeCard";
export {default as UpsertSizeModal} from "./components/size/UpsertSizeModal";

//Product
export {default as ProductTable} from "./components/product/ProductTable";
export {default as UpsertProductModal} from "./components/product/UpsertProductModal";
export {default as ProductOverview} from "./components/product/ProductOverview";

//Product Variant
export {default as VariantTable} from "./components/productVariant/VariantTable";
export {default as UpsertProductVariantModal} from "./components/productVariant/UpsertProductVariantModal";

//Product Image
export {default as ProductImageModal} from "./components/productImage/ProductImageModal";

//hooks
//Category
export {useGetCategories} from "./hooks/category/useGetCategories";
export {useCreateCategory} from "./hooks/category/useCreateCategory";
export {useUpdateCategory} from "./hooks/category/useUpdateCategory";
export {useDeleteCategory} from "./hooks/category/useDeleteCategory";

//Color
export {useGetColors} from "./hooks/color/useGetColors";
export {useCreateColor} from "./hooks/color/useCreateColor";
export {useUpdateColor} from "./hooks/color/useUpdateColor";
export {useDeleteColor} from "./hooks/color/useDeleteColor";

//Size
export {useGetSizes} from "./hooks/size/useGetSizes";
export {useCreateSize} from "./hooks/size/useCreateSize";
export {useUpdateSize} from "./hooks/size/useUpdateSize";
export {useDeleteSize} from "./hooks/size/useDeleteSize";   

//Product
export {useGetProducts} from "./hooks/product/useGetProducts";
export {useGetProductDetailById} from "./hooks/product/useGetProductDetailById";
export {useCreateProduct} from "./hooks/product/useCreateProduct";
export {useUpdateProduct} from "./hooks/product/useUpdateProduct";
export {useDeleteProduct} from "./hooks/product/useDeleteProduct";

//Product Variant
export {useCreateProductVariant} from "./hooks/productVariant/useCreateProductVariant";
export {useUpdateProductVariant} from "./hooks/productVariant/useUpdateProductVariant";
export {useDeleteProductVariant} from "./hooks/productVariant/useDeleteProductVariant";

//Product Image
export {useGetProductImages} from "./hooks/productImages/useGetProductImages";
export {useCreateProductImages} from "./hooks/productImages/useCreateProductImages";
export {useUpdateProductImage} from "./hooks/productImages/useUpdateProductImage";
export {useDeleteProductImage} from "./hooks/productImages/useDeleteProductImage";
