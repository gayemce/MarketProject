import { Money } from "./product.model";

export class AddShoppingCartModel {
    productId: number = 0;
    price: Money = new Money();
    quantity: number = 0;
    userId: number = 0
}