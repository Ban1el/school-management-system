import { BaseDto } from '../Common/BaseDto';

export type RoleDto = BaseDto & {
  id: number;
  name: string;
};
