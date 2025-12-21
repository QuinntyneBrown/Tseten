import { AcceptanceCriteriaStatus } from './acceptance-criteria-status';
import { AcceptanceCriteriaPriority } from './acceptance-criteria-priority';

export interface AcceptanceCriteria {
  acceptanceCriteriaId: string;
  given: string;
  when: string;
  then: string;
  status: AcceptanceCriteriaStatus;
  priority: AcceptanceCriteriaPriority;
  notes: string;
  createdAt: Date;
  updatedAt?: Date;
}
