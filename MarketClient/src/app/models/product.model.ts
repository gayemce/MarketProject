export class ProductModel{
    id: number = 0;
    name: string = "";
    brand: string = "";
    img: string = "";
    description: string = "";
    price: Money = new Money();
    stock: number = 0;
    barcode: string = "";
    isActive: boolean = true;
    categoryId: number = 0;
    isdelete: boolean = false;
    categories: string[] = [];
}

export class Money{
    value: number = 0;
    currency: string = "₺";
}