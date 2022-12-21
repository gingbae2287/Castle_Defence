namespace st{
    //영웅의 최종 스텟
    public struct FinalStat{
        public int damage;
        public float atkSpeed;
        public FinalStat(HeroStat baseStat){
            damage=baseStat.damage;
            atkSpeed=baseStat.atkSpeed;
        }
        public void StatInit(HeroStat baseStat){
            //영웅 등록 해제시, 적용중인 다른 영웅들의 패시브 효과를 없엔 순수 stat으로 적용
            damage=baseStat.damage;
            atkSpeed=baseStat.atkSpeed;
        }
        public void SetFinalStat(HeroStat baseStat){
            StatInit(baseStat);
            damage=(int)(damage*(1+SkillManager.p_TeamBuff.DamageIncRate)+SkillManager.p_TeamBuff.DamageInc);
            atkSpeed=atkSpeed*(1+SkillManager.p_TeamBuff.AtkSpeedIncRate)+SkillManager.p_TeamBuff.AtkSpeedInc;
            
        }
    }

    public struct TeamBuff{
    /// team buff skill values
    //  Inc=Increase
        private float damageIncRate;
        public float DamageIncRate{get{return damageIncRate;} set{damageIncRate=value;}}
        private int damageInc;
        public int DamageInc{get{return damageInc;} set{damageInc=value;}}
        private float atkSpeedIncRate;
        public float AtkSpeedIncRate{get{return atkSpeedIncRate;} set{atkSpeedIncRate=value;}}
        private float atkSpeedInc;
        public float AtkSpeedInc{get{return atkSpeedInc;} set{atkSpeedInc=value;}}
        public void Init(){
            damageIncRate=0;
            damageInc=0;
            atkSpeedIncRate=0;
            atkSpeedInc=0;
        
        }
    }

    
}