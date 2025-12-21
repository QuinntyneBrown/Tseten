import { AcceptanceCriteria } from './acceptance-criteria';
import { AcceptanceCriteriaStatus } from './acceptance-criteria-status';
import { AcceptanceCriteriaPriority } from './acceptance-criteria-priority';

describe('AcceptanceCriteria', () => {
  it('should create an acceptance criteria object', () => {
    const acceptanceCriteria: AcceptanceCriteria = {
      acceptanceCriteriaId: '1',
      given: 'Given a user is logged in',
      when: 'When the user clicks save',
      then: 'Then the data should be persisted',
      status: AcceptanceCriteriaStatus.Pending,
      priority: AcceptanceCriteriaPriority.Medium,
      notes: 'Test notes',
      createdAt: new Date(),
      updatedAt: undefined
    };

    expect(acceptanceCriteria.acceptanceCriteriaId).toBe('1');
    expect(acceptanceCriteria.given).toBe('Given a user is logged in');
    expect(acceptanceCriteria.when).toBe('When the user clicks save');
    expect(acceptanceCriteria.then).toBe('Then the data should be persisted');
    expect(acceptanceCriteria.status).toBe(AcceptanceCriteriaStatus.Pending);
    expect(acceptanceCriteria.priority).toBe(AcceptanceCriteriaPriority.Medium);
    expect(acceptanceCriteria.notes).toBe('Test notes');
    expect(acceptanceCriteria.createdAt).toBeDefined();
    expect(acceptanceCriteria.updatedAt).toBeUndefined();
  });

  it('should support all status values', () => {
    const statuses = [
      AcceptanceCriteriaStatus.Pending,
      AcceptanceCriteriaStatus.Passed,
      AcceptanceCriteriaStatus.Failed,
      AcceptanceCriteriaStatus.NotApplicable
    ];

    statuses.forEach((status, index) => {
      expect(status).toBe(index);
    });
  });

  it('should support all priority values', () => {
    const priorities = [
      AcceptanceCriteriaPriority.Low,
      AcceptanceCriteriaPriority.Medium,
      AcceptanceCriteriaPriority.High,
      AcceptanceCriteriaPriority.Critical
    ];

    priorities.forEach((priority, index) => {
      expect(priority).toBe(index);
    });
  });

  it('should allow updatedAt to be optional', () => {
    const acceptanceCriteria: AcceptanceCriteria = {
      acceptanceCriteriaId: '1',
      given: 'Given',
      when: 'When',
      then: 'Then',
      status: AcceptanceCriteriaStatus.Pending,
      priority: AcceptanceCriteriaPriority.Low,
      notes: '',
      createdAt: new Date()
    };

    expect(acceptanceCriteria.updatedAt).toBeUndefined();
  });
});
