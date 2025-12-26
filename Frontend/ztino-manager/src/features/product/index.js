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
