using System;

public interface IEnemyState
{
	void SetContext(Enemy context);
	void AddTransition(Func<bool> condition, IEnemyState newState);
	bool IsDone();
	void OnEnter();
	void CheckTransitions();
	void Update();
	void OnExit();
}
