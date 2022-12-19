namespace EnumTypes
{
	public enum GameState
	{
		Idle, Wave
	}

	///======
	///	Skill
	///======
	public enum SkillType{
		ActiveSoloBuff,
		PassiveTeamBuff,

	}
	/// Passive Team Buff Skill Type
	public enum PTBSkillType{
		DamageIncreaseRate,
		DamageIncreaseValue,
		AttackSpeedIncreaseRate,
		AttackSpeedIncreaseValue
	};

}