import { SoftwareRequirement } from './software-requirement';
import { AcceptanceCriteria } from './acceptance-criteria';
import { AcceptanceCriteriaStatus } from './acceptance-criteria-status';
import { AcceptanceCriteriaPriority } from './acceptance-criteria-priority';

describe('SoftwareRequirement', () => {
  it('should create a software requirement with acceptance criteria', () => {
    const acceptanceCriteria: AcceptanceCriteria = {
      acceptanceCriteriaId: 'AC-001',
      given: 'Given a user is logged in',
      when: 'When the user clicks save',
      then: 'Then the data should be persisted',
      status: AcceptanceCriteriaStatus.Pending,
      priority: AcceptanceCriteriaPriority.High,
      notes: '',
      createdAt: new Date()
    };

    const softwareRequirement: SoftwareRequirement = {
      softwareRequirementId: 'REQ-001',
      parentSoftwareRequirementId: '',
      description: 'Test requirement with acceptance criteria',
      canImplement: true,
      canTest: true,
      comments: [],
      acceptanceCriteria: [acceptanceCriteria]
    };

    expect(softwareRequirement.softwareRequirementId).toBe('REQ-001');
    expect(softwareRequirement.description).toBe('Test requirement with acceptance criteria');
    expect(softwareRequirement.acceptanceCriteria.length).toBe(1);
    expect(softwareRequirement.acceptanceCriteria[0].given).toBe('Given a user is logged in');
  });

  it('should support multiple acceptance criteria', () => {
    const softwareRequirement: SoftwareRequirement = {
      softwareRequirementId: 'REQ-001',
      parentSoftwareRequirementId: '',
      description: 'Test requirement',
      canImplement: true,
      canTest: true,
      comments: [],
      acceptanceCriteria: [
        {
          acceptanceCriteriaId: 'AC-001',
          given: 'Given 1',
          when: 'When 1',
          then: 'Then 1',
          status: AcceptanceCriteriaStatus.Pending,
          priority: AcceptanceCriteriaPriority.Low,
          notes: '',
          createdAt: new Date()
        },
        {
          acceptanceCriteriaId: 'AC-002',
          given: 'Given 2',
          when: 'When 2',
          then: 'Then 2',
          status: AcceptanceCriteriaStatus.Passed,
          priority: AcceptanceCriteriaPriority.High,
          notes: '',
          createdAt: new Date()
        },
        {
          acceptanceCriteriaId: 'AC-003',
          given: 'Given 3',
          when: 'When 3',
          then: 'Then 3',
          status: AcceptanceCriteriaStatus.Failed,
          priority: AcceptanceCriteriaPriority.Critical,
          notes: '',
          createdAt: new Date()
        }
      ]
    };

    expect(softwareRequirement.acceptanceCriteria.length).toBe(3);
    expect(softwareRequirement.acceptanceCriteria[0].status).toBe(AcceptanceCriteriaStatus.Pending);
    expect(softwareRequirement.acceptanceCriteria[1].status).toBe(AcceptanceCriteriaStatus.Passed);
    expect(softwareRequirement.acceptanceCriteria[2].status).toBe(AcceptanceCriteriaStatus.Failed);
  });

  it('should support empty acceptance criteria list', () => {
    const softwareRequirement: SoftwareRequirement = {
      softwareRequirementId: 'REQ-001',
      parentSoftwareRequirementId: '',
      description: 'Test requirement without acceptance criteria',
      canImplement: true,
      canTest: true,
      comments: [],
      acceptanceCriteria: []
    };

    expect(softwareRequirement.acceptanceCriteria.length).toBe(0);
  });

  it('should support parent-child relationships', () => {
    const childRequirement: SoftwareRequirement = {
      softwareRequirementId: 'REQ-002',
      parentSoftwareRequirementId: 'REQ-001',
      description: 'Child requirement',
      canImplement: true,
      canTest: true,
      comments: [],
      acceptanceCriteria: []
    };

    expect(childRequirement.parentSoftwareRequirementId).toBe('REQ-001');
  });
});
