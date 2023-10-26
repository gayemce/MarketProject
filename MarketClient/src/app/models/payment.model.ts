import { ProductModel } from "./product.model";

export class PaymentModel {
    products: ProductModel[] = []
    buyer: BuyerModel = new BuyerModel();
    shippingAddress: AddressModel = new AddressModel();
    billingAddress: AddressModel = new AddressModel();
    paymentCard: PaymentCardModel = new PaymentCardModel();
}

export class BuyerModel {
    id: string = "";
    name: string = "Gaye";
    surname: string = "T.";
    identityNumber: string = "12345678901";
    email: string = "gayettekn@gmail.com";
    gsmNumber: string = "5416022536";
    registrationDate: string = "";
    lastLoginDate: string = "";
    registrationAddress: string = "";
    city: string = "";
    country: string = "";
    zipCode: string = "";
    ip: string = "";
}

export class AddressModel {
    description: string = "Ankara";
    zipCode: string = "06000";
    contactName: string = "Gaye T.";
    city: string = "Ankara";
    country: string = "Türkiye";
}

export class PaymentCardModel {
    cardHolderName: string = "Gaye Tekin";
    cardNumber: string = "";
    expireYear: string = "";
    expireMonth: string = "";
    cvc: string = "377";
}