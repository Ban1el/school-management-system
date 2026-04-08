import { BaseDto } from '../Common/BaseDto';

export type CityMunicipalityDto = BaseDto & {
  id: number;
  name: string;
  provinceId: number | null;
  regionId: number | null;
};
