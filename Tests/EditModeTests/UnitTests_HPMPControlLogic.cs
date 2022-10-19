using System.Collections.Generic;
using NUnit.Framework;

public class UnitTests_HPMPControlLogic
{
    public class DamageTestCase
    {
        public string TestName { get; private set; }
        public int StartHealth { get; private set; }
        public int Damage { get; private set; }
        public int ExpectedHealth { get; private set; }
        public bool ExpectedDeath { get; private set; }

        public DamageTestCase(int startHealth, int damage, int expectedHealth, bool expectedDeath)
        {
            ExpectedHealth = expectedHealth;
            Damage = damage;
            StartHealth = startHealth;
            ExpectedDeath = expectedDeath;
        }

        public override string ToString()
        {
            return TestName;
        }
    }

    private static IEnumerable<DamageTestCase> DamageTestCases
    {
        get
        {
            yield return new DamageTestCase(5, 2, 3, false);
            yield return new DamageTestCase(5, 5, 0, true);
            yield return new DamageTestCase(5, 7, -2, true);
        }
    }

    [Test]
    [TestCaseSource("DamageTestCases")]
    public void TakeDamage_DealsDamageToEnemy(DamageTestCase damageTestCase)
    {
        var enemyHPMPLogic = new HPMPControlLogic(damageTestCase.StartHealth);

        enemyHPMPLogic.TakeDamage(damageTestCase.Damage);

        Assert.AreEqual(damageTestCase.ExpectedHealth, enemyHPMPLogic.currentHP);
    }

    [Test]
    [TestCaseSource("DamageTestCases")]
    public void TakeDamage_EnemyDiesWithHPEqualZeroOrSmaller(DamageTestCase damageTestCase)
    {
        var enemyHPMPLogic = new HPMPControlLogic(damageTestCase.StartHealth);

        enemyHPMPLogic.TakeDamage(damageTestCase.Damage);

        Assert.AreEqual(damageTestCase.ExpectedDeath, enemyHPMPLogic.dead);
    }
}
