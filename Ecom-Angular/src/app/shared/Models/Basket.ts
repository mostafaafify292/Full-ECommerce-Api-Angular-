import { v4 as uuidv4 } from 'uuid';

export interface IBasket {
  Id: string;
  basketItems: IBasketItem[];
}

export interface IBasketItem {
  id: string;
  name: string;
  quantity: number;
  imageURL: string;
  price: number;
  category: string;
}
export class Basket implements IBasket {
  Id = uuidv4();
  basketItems: IBasketItem[]=[];
}
