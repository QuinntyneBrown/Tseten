import { AcceptanceCriteria } from './acceptance-criteria';
import { Comment } from './comment';

export interface SoftwareRequirement {
  softwareRequirementId: string;
  parentSoftwareRequirementId: string;
  description: string;
  canImplement: boolean;
  canTest: boolean;
  comments: Comment[];
  acceptanceCriteria: AcceptanceCriteria[];
}
