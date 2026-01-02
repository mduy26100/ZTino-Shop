    import React, { useState, useEffect, memo } from 'react';
    import { Image } from 'antd';

    const ProductGallery = ({ images = [] }) => {
        const [mainImage, setMainImage] = useState(images[0]?.imageUrl);

        useEffect(() => {
            if (images.length > 0) {
                const primaryImg = images.find(img => img.isMain)?.imageUrl || images[0].imageUrl;
                setMainImage(primaryImg);
            } else {
                setMainImage(null);
            }
        }, [images]);

        if (!images.length) {
            return (
                <div className="bg-gray-50 aspect-[3/4] rounded-2xl flex items-center justify-center text-gray-400">
                    No images available
                </div>
            );
        }

        return (
            <div className="flex flex-col-reverse lg:flex-row gap-4">
                <div className="flex lg:flex-col gap-4 overflow-x-auto lg:overflow-y-auto lg:h-[600px] scrollbar-hide py-2 lg:py-0 px-1 lg:px-0">
                    {images.map((img, index) => (
                        <div 
                            key={img.id || index}
                            className={`
                                flex-shrink-0 w-20 h-24 cursor-pointer rounded-lg overflow-hidden border-2 transition-all duration-300
                                ${mainImage === img.imageUrl ? 'border-indigo-600 shadow-md scale-95' : 'border-transparent hover:border-gray-200'}
                            `}
                            onMouseEnter={() => setMainImage(img.imageUrl)}
                            onClick={() => setMainImage(img.imageUrl)}
                        >
                            <img 
                                src={img.imageUrl} 
                                alt={`thumbnail-${index}`} 
                                className="w-full h-full object-cover" 
                                loading="lazy"
                            />
                        </div>
                    ))}
                </div>

                <div className="flex-1 bg-gray-50 rounded-2xl overflow-hidden relative group aspect-[3/4] lg:aspect-auto lg:h-[600px]">
                    <Image
                        src={mainImage}
                        className="w-full h-full object-cover object-center"
                        alt="product-main"
                        wrapperClassName="w-full h-full"
                        preview={{ mask: <div className="text-white text-sm font-medium">Click to Zoom</div> }}
                    />
                </div>
            </div>
        );
    };

    export default memo(ProductGallery);