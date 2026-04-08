import { BaseDto } from '../Common/BaseDto';

export type BarangayDto = BaseDto & {
  id: number;
  name: string;
  cityId: number | null;
};
