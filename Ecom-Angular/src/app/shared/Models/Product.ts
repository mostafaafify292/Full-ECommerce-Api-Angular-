export interface IProduct {
  id: number;
  name: string;
  description: string;
  newPrice: number;
  oldPrice: number;
  photos: IPhoto[];
  categoryName: string;
}

export interface IPhoto {
  imageName: string;
  productId: number;
}
