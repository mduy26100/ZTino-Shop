import React, { memo, useState, useCallback } from 'react';
import { Slider, Select, Typography, Divider, Button, InputNumber, Row, Col } from 'antd';
import { FilterOutlined } from '@ant-design/icons';

const { Text } = Typography;

const ProductFilter = () => {
    const [priceRange, setPriceRange] = useState([0, 5000000]);
    const maxPrice = 10000000;

    const handleSliderChange = useCallback((value) => {
        setPriceRange(value);
    }, []);

    const handleMinInput = useCallback((value) => {
        const newMin = value === null ? 0 : value;
        if (newMin <= priceRange[1]) {
            setPriceRange([newMin, priceRange[1]]);
        }
    }, [priceRange]);

    const handleMaxInput = useCallback((value) => {
        const newMax = value === null ? maxPrice : value;
        if (newMax >= priceRange[0]) {
            setPriceRange([priceRange[0], newMax]);
        }
    }, [priceRange]);

    return (
        <div className="w-full">
            <div className="flex items-center gap-2 mb-6">
                <FilterOutlined className="text-lg" />
                <span className="text-base font-medium uppercase tracking-wide">Filters</span>
            </div>

            <div className="mb-8">
                <Text className="text-xs font-bold uppercase tracking-wider text-gray-500 mb-3 block">Price Range</Text>
                
                <Slider 
                    range 
                    min={0}
                    max={maxPrice}
                    value={priceRange}
                    onChange={handleSliderChange}
                    tooltip={{ formatter: value => `${value?.toLocaleString()}đ` }}
                    className="mb-6"
                    trackStyle={[{ backgroundColor: '#000' }]}
                    handleStyle={[
                        { borderColor: '#000', backgroundColor: '#fff' },
                        { borderColor: '#000', backgroundColor: '#fff' }
                    ]}
                />

                <Row gutter={16} align="middle">
                    <Col span={11}>
                        <InputNumber
                            min={0}
                            max={maxPrice}
                            value={priceRange[0]}
                            onChange={handleMinInput}
                            style={{ width: '100%' }}
                            formatter={value => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                            parser={value => value?.replace(/\$\s?|(,*)/g, '')}
                            className="rounded-none border-gray-300 focus:border-black hover:border-black text-xs"
                            placeholder="Min"
                        />
                    </Col>
                    <Col span={2} className="text-center text-gray-400">
                        -
                    </Col>
                    <Col span={11}>
                        <InputNumber
                            min={0}
                            max={maxPrice}
                            value={priceRange[1]}
                            onChange={handleMaxInput}
                            style={{ width: '100%' }}
                            formatter={value => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                            parser={value => value?.replace(/\$\s?|(,*)/g, '')}
                            className="rounded-none border-gray-300 focus:border-black hover:border-black text-xs"
                            placeholder="Max"
                        />
                    </Col>
                </Row>
            </div>

            <Divider className="my-6" />

            <div className="mb-8">
                <Text className="text-xs font-bold uppercase tracking-wider text-gray-500 mb-3 block">Sort By</Text>
                <Select
                    defaultValue="newest"
                    style={{ width: '100%' }}
                    bordered={false}
                    className="border-b border-gray-200 hover:border-gray-400 transition-colors rounded-none px-0"
                    dropdownStyle={{ borderRadius: 0 }}
                    options={[
                        { value: 'newest', label: 'Newest Arrivals' },
                        { value: 'price_asc', label: 'Price: Low to High' },
                        { value: 'price_desc', label: 'Price: High to Low' },
                        { value: 'oldest', label: 'Oldest Items' },
                    ]}
                />
            </div>

            <Button type="primary" block className="bg-black hover:!bg-slate-800 border-none h-10 uppercase tracking-widest text-xs font-bold rounded-none">
                Apply Filters
            </Button>
        </div>
    );
};

export default memo(ProductFilter);