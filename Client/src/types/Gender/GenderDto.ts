import { BaseDto } from '../Common/BaseDto';

export type GenderDto = BaseDto & {
  id: number;
  name: string;
};
