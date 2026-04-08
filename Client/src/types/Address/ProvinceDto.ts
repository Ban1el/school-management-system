import { BaseDto } from '../Common/BaseDto';

export type ProvinceDto = BaseDto & {
  id: number;
  name: string;
  regionId: number;
};
