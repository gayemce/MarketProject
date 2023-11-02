import { Money } from "./product.model";

export class SetShoppingCartsModel{
    productId: number = 0;
    userId: number = 0;
    quantity: number = 0;
    price: Money = new Money;
}