using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RPSCardsEngine.API;
using RPSCardsEngine.API.Cards;
using RPSCardsEngine.API.CardZone;
using RPSCardsEngine.Cards;
using RPSCardsEngine.UnitTests.Cards.DummyObjects;
using System;

namespace RPSCardsEngine.UnitTests.Cards
{
    [TestClass]
    public class CardTest
    {
        readonly CardType rock = CardType.Rock;
        readonly CardType fire = CardType.Fire;
        readonly CardType water = CardType.Water;
        readonly CardType magma = CardType.Magma;

        ICardTypeHelper cardTypeHelper;

        [ClassInitialize]
        public void Setup(TestContext context)
        {
            var cardTypeHelperMock = new Mock<ICardTypeHelper>();
            cardTypeHelperMock.Setup(helper => 
            helper.IsElement(It.IsAny<CardType>()))
                .Returns((CardType type) => (type == fire));

            cardTypeHelperMock.Setup(helper => 
            helper.IsUnit(It.IsAny<CardType>()))
                .Returns((CardType type) => (type == rock || type == magma));

            cardTypeHelperMock.Setup(helper => 
            helper.IsBaseUnit(It.IsAny<CardType>()))
                .Returns((CardType type) => (type == rock));

            cardTypeHelperMock.Setup(helper => 
            helper.IsUpgradedUnit(It.IsAny<CardType>()))
                .Returns((CardType type) => (type == magma));

            cardTypeHelperMock.Setup(helper => 
            helper.DoesElementDestroy(It.IsAny<CardType>(), It.IsAny<CardType>()))
                .Returns((CardType element, CardType type) => 
                (element == water && type == rock));

            cardTypeHelperMock.Setup(helper =>
            helper.DoesUnitDestroy(It.IsAny<CardType>(), It.IsAny<CardType>()))
                .Returns((CardType attacker, CardType type) =>
                (attacker == magma && type == rock));

            cardTypeHelperMock.Setup(helper =>
            helper.DoesElementUpgrade(It.IsAny<CardType>(), It.IsAny<CardType>()))
                .Returns((CardType element, CardType type) =>
                (element == magma && type == rock));
            
            cardTypeHelperMock.Setup(helper =>
            helper.GetBaseUnit(It.IsAny<CardType>()))
                .Returns((CardType type) => rock);

            cardTypeHelperMock.Setup(helper =>
            helper.GetBaseUnit(It.IsAny<CardType>()))
                .Returns((CardType type) => fire);

            cardTypeHelper = cardTypeHelperMock.Object;
            Card.SetCardTypeHelper(cardTypeHelperMock.Object);
        }


        [TestMethod]
        public void Constructor_CreateBasicUnit_SetsPropperties()
        {
            ICard card = new Card(rock, Zone.Hand, 0);

            Assert.AreEqual(rock, card.Type);
            Assert.AreEqual(Zone.Hand, card.Zone);
            Assert.AreEqual(0, card.Position);
            Assert.IsFalse(card.IsDestroyed);
        }

        [TestMethod]
        public void Constructor_CreateElement_SetsPropperties()
        {
            ICard card = new Card(fire, Zone.Deck, 2);

            Assert.AreEqual(fire, card.Type);
            Assert.AreEqual(Zone.Deck, card.Zone);
            Assert.AreEqual(2, card.Position);
            Assert.IsFalse(card.IsDestroyed);
        }

        [TestMethod]
        public void Constructor_CreateUpgradedUnit_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>
                (() =>new Card(magma, Zone.Deck, 2));
        }


        public void AddElement_UpgradeHelper(CardType original, CardType element, CardType result)
        {
            var c = new RPSCardsEngine.Cards.Card(original, Zone.Board, 0);

            c.AddElement(new RPSCardsEngine.Cards.Card(element));

            Assert.AreEqual(c.Type, result);
            Assert.AreEqual(c.Zone, Zone.Board);
            Assert.AreEqual(c.Position, 0);
            Assert.IsFalse(c.IsDestroyed);
        }

        [TestMethod]
        public void TestChangeZone()
        {
            var c = new Card(CardType.Fire, Zone.Hand, 0);

            c.ChangeZone(Zone.Board, 1);

            Assert.AreEqual(c.Type, CardType.Fire);
            Assert.AreEqual(c.Zone, Zone.Board);
            Assert.AreEqual(c.Position, 1);
            Assert.IsFalse(c.IsDestroyed);
        }

        #region AddElementUpgradeTests

        public void TestAddElement_UpgradeHelper(CardType original, CardType element, CardType result)
        {
            var c = new RPSCardsEngine.Cards.Card(original, Zone.Board, 0);

            c.AddElement(new RPSCardsEngine.Cards.Card(element));

            Assert.AreEqual(c.Type, result);
            Assert.AreEqual(c.Zone, Zone.Board);
            Assert.AreEqual(c.Position, 0);
            Assert.IsFalse(c.IsDestroyed);
        }

        [TestMethod]
        public void TestAddElement_UpgradeRockFire() 
            => TestAddElement_UpgradeHelper(CardType.Rock, CardType.Fire, CardType.Magma);

        [TestMethod]
        public void TestAddElement_UpgradeRockWood() 
            => TestAddElement_UpgradeHelper(CardType.Rock, CardType.Wood, CardType.Axe);
        
        [TestMethod]
        public void TestAddElement_UpgradePaperWater() 
            => TestAddElement_UpgradeHelper(CardType.Paper, CardType.Water, CardType.Glass);

        [TestMethod]
        public void TestAddElement_UpgradePaperWood() 
            => TestAddElement_UpgradeHelper(CardType.Paper, CardType.Wood, CardType.Tree);
        
        [TestMethod]
        public void TestAddElement_UpgradeScissorFire() 
            => TestAddElement_UpgradeHelper(CardType.Scissor, CardType.Fire, CardType.Laser);
        
        [TestMethod]
        public void TestAddElement_UpgradeScissorWater() 
            => TestAddElement_UpgradeHelper(CardType.Scissor, CardType.Water, CardType.WaterGun);

        #endregion

        #region AddElementDestroyTests

        public void TestAddElement_DestroyHelper(CardType original, CardType element)
        {
            var c = new RPSCardsEngine.Cards.Card(CardTypeHelpers.GetBaseUnit(original), Zone.Board, 0);

            if (CardTypeHelpers.IsBaseUnit(original) == false)
                c.AddElement(new RPSCardsEngine.Cards.Card(CardTypeHelpers.GetUnitElement(original)));

            c.AddElement(new RPSCardsEngine.Cards.Card(element));

            Assert.AreEqual(c.Type, original);
            Assert.AreEqual(c.Zone, Zone.Board);
            Assert.AreEqual(c.Position, 0);
            Assert.IsTrue(c.IsDestroyed);
        }

        [TestMethod]
        public void TestAddElement_DestroyRockWater() => TestAddElement_DestroyHelper(CardType.Rock, CardType.Water);

        [TestMethod]
        public void TestAddElement_DestroyMagmaWater() => TestAddElement_DestroyHelper(CardType.Magma, CardType.Water);

        [TestMethod]
        public void TestAddElement_DestroyLaserWater() => TestAddElement_DestroyHelper(CardType.Laser, CardType.Water);

        [TestMethod]
        public void TestAddElement_DestroyPaperFire() => TestAddElement_DestroyHelper(CardType.Paper, CardType.Fire);

        [TestMethod]
        public void TestAddElement_DestroyAxeFire() => TestAddElement_DestroyHelper(CardType.Axe, CardType.Fire);

        [TestMethod]
        public void TestAddElement_DestroyTreeFire() => TestAddElement_DestroyHelper(CardType.Tree, CardType.Fire);

        [TestMethod]
        public void TestAddElement_DestroyScissorWood() => TestAddElement_DestroyHelper(CardType.Scissor, CardType.Wood);

        [TestMethod]
        public void TestAddElement_DestroyGlassWood() => TestAddElement_DestroyHelper(CardType.Glass, CardType.Wood);

        [TestMethod]
        public void TestAddElement_DestroyWatergunWood() => TestAddElement_DestroyHelper(CardType.WaterGun, CardType.Wood);

        #endregion

        //#region FightTests
        //private void TestFight(CardType winner, CardType loser)
        //{
        //    if (CardTypeHelpers.IsUnit(winner) == false &&
        //        CardTypeHelpers.IsUnit(loser) == false)
        //        throw new ArgumentException("CardTypes must be units");

        //    Card winCard = TestHelpers.CardOfType(winner, Zone.Board);
        //    Card winCardCopy = TestHelpers.CardOfType(winner, Zone.Board);
        //    Card loseCard = TestHelpers.CardOfType(loser, Zone.Board);
        //    Card loseCardCopy = TestHelpers.CardOfType(loser, Zone.Board);

        //    winCard.Fight(loseCard);

        //    Assert.IsTrue(loseCard.IsDestroyed);
        //    Assert.IsFalse(winCard.IsDestroyed);

        //    Assert.AreEqual(winCard.Type, winCardCopy.Type);
        //    Assert.AreEqual(winCard.Zone, winCardCopy.Zone);
        //    Assert.AreEqual(winCard.Position, winCardCopy.Position);

        //    Assert.AreEqual(loseCard.Type, loseCardCopy.Type);
        //    Assert.AreEqual(loseCard.Zone, loseCardCopy.Zone);
        //    Assert.AreEqual(loseCard.Position, loseCardCopy.Position);
        //}
        
        //private void TestFightSelf(CardType type)
        //{
        //    if (CardTypeHelpers.Units.Contains(type) == false)
        //        throw new ArgumentException("CardType must be units");

        //    Card firstCard = TestHelpers.CardOfType(type, Zone.Board);
        //    Card firstCardCopy = TestHelpers.CardOfType(type, Zone.Board);
        //    Card secondCard = TestHelpers.CardOfType(type, Zone.Board);
        //    Card SecondCardCopy = TestHelpers.CardOfType(type, Zone.Board);

        //    firstCard.Fight(secondCard);

        //    Assert.AreEqual(firstCard.Type, firstCardCopy.Type);
        //    Assert.AreEqual(firstCard.Zone, firstCardCopy.Zone);
        //    Assert.AreEqual(firstCard.Position, firstCardCopy.Position);

        //    Assert.AreEqual(secondCard.Type, SecondCardCopy.Type);
        //    Assert.AreEqual(secondCard.Zone, SecondCardCopy.Zone);
        //    Assert.AreEqual(secondCard.Position, SecondCardCopy.Position);
        //}

        //[TestMethod]
        //public void TestFightRock()
        //{
        //    CardType thisType = CardType.Rock;
        //    TestFight(thisType, CardType.Scissor);
        //    TestFight(thisType, CardType.Laser);
        //    TestFight(thisType, CardType.WaterGun);
        //    TestFight(CardType.Magma, thisType);
        //    TestFight(CardType.Axe, thisType);
        //    TestFight(CardType.Paper, thisType);
        //    TestFight(CardType.Glass, thisType);
        //    TestFight(CardType.Tree, thisType);
        //    TestFightSelf(thisType);
        //}

        //[TestMethod]
        //public void TestFightPaper()
        //{
        //    CardType thisType = CardType.Paper;
        //    TestFight(thisType, CardType.Rock);
        //    TestFight(thisType, CardType.Axe);
        //    TestFight(thisType, CardType.Magma);
        //    TestFight(CardType.Glass, thisType);
        //    TestFight(CardType.Tree, thisType);
        //    TestFight(CardType.Scissor, thisType);
        //    TestFight(CardType.Laser, thisType);
        //    TestFight(CardType.WaterGun, thisType);
        //    TestFightSelf(thisType);
        //}

        //[TestMethod]
        //public void TestFightScissor()
        //{
        //    CardType thisType = CardType.Scissor;
        //    TestFight(thisType, CardType.Paper);
        //    TestFight(thisType, CardType.Glass);
        //    TestFight(thisType, CardType.Tree);
        //    TestFight(CardType.Magma, thisType);
        //    TestFight(CardType.Axe, thisType);
        //    TestFight(CardType.Rock, thisType);
        //    TestFight(CardType.Laser, thisType);
        //    TestFight(CardType.WaterGun, thisType);
        //    TestFightSelf(thisType);
        //}

        //[TestMethod]
        //public void TestFightMagma()
        //{
        //    CardType thisType = CardType.Magma;
        //    TestFight(thisType, CardType.Rock);
        //    TestFight(thisType, CardType.Axe);
        //    TestFight(thisType, CardType.Laser);
        //    TestFight(thisType, CardType.Tree);
        //    TestFight(thisType, CardType.Scissor);
        //    TestFight(CardType.Paper, thisType);
        //    TestFight(CardType.Glass, thisType);
        //    TestFight(CardType.WaterGun, thisType);
        //    TestFightSelf(thisType);
        //}

        //[TestMethod]
        //public void TestFightAxe()
        //{
        //    CardType thisType = CardType.Axe;
        //    TestFight(thisType, CardType.Rock);
        //    TestFight(thisType, CardType.Glass);
        //    TestFight(thisType, CardType.WaterGun);
        //    TestFight(CardType.Magma, thisType);
        //    TestFight(CardType.Paper, thisType);
        //    TestFight(CardType.Tree, thisType);
        //    TestFight(CardType.Laser, thisType);
        //    TestFightSelf(thisType);
        //}

        //[TestMethod]
        //public void TestFightTree()
        //{
        //    CardType thisType = CardType.Tree;
        //    TestFight(thisType, CardType.Paper);
        //    TestFight(thisType, CardType.Glass);
        //    TestFight(thisType, CardType.Axe);
        //    TestFight(thisType, CardType.Rock);
        //    TestFight(thisType, CardType.WaterGun);
        //    TestFight(CardType.Magma, thisType);
        //    TestFight(CardType.Scissor, thisType);
        //    TestFight(CardType.Laser, thisType);
        //    TestFightSelf(thisType);
        //}

        //[TestMethod]
        //public void TestFightGlass()
        //{
        //    CardType thisType = CardType.Glass;
        //    TestFight(thisType, CardType.Paper);
        //    TestFight(thisType, CardType.Magma);
        //    TestFight(thisType, CardType.Rock);
        //    TestFight(thisType, CardType.Laser);
        //    TestFight(CardType.Scissor, thisType);
        //    TestFight(CardType.Axe, thisType);
        //    TestFight(CardType.Tree, thisType);
        //    TestFight(CardType.WaterGun, thisType);
        //    TestFightSelf(thisType);
        //}

        //[TestMethod]
        //public void TestFightWaterGun()
        //{
        //    CardType thisType = CardType.WaterGun;
        //    TestFight(thisType, CardType.Scissor);
        //    TestFight(thisType, CardType.Glass);
        //    TestFight(thisType, CardType.Magma);
        //    TestFight(thisType, CardType.Laser);
        //    TestFight(thisType, CardType.Paper);
        //    TestFight(CardType.Tree, thisType);
        //    TestFight(CardType.Axe, thisType);
        //    TestFight(CardType.Rock, thisType);
        //    TestFightSelf(thisType);
        //}

        //[TestMethod]
        //public void TestFightLaser()
        //{
        //    CardType thisType = CardType.Laser;
        //    TestFight(thisType, CardType.Scissor);
        //    TestFight(thisType, CardType.Axe);
        //    TestFight(thisType, CardType.Tree);
        //    TestFight(thisType, CardType.Paper);
        //    TestFight(CardType.Magma, thisType);
        //    TestFight(CardType.Glass, thisType);
        //    TestFight(CardType.Rock, thisType);
        //    TestFight(CardType.WaterGun, thisType);
        //    TestFightSelf(thisType);
        //}

        //#endregion

        [TestMethod]
        public void TestAddElement_Error()
        {
            var card1 = new RPSCardsEngine.Cards.Card(CardType.Fire, Zone.Board, 0);
            Assert.ThrowsException<InvalidOperationException>(() => card1.AddElement(new RPSCardsEngine.Cards.Card(CardType.Water)));

            var card2 = new RPSCardsEngine.Cards.Card(CardType.Rock, Zone.Board, 0);
            Assert.ThrowsException<ArgumentException>(() => card2.AddElement(new RPSCardsEngine.Cards.Card(CardType.Rock)));

            card2.AddElement(new RPSCardsEngine.Cards.Card(CardType.Water));
            Assert.IsTrue(card2.IsDestroyed);
            Assert.ThrowsException<InvalidOperationException>(() => card2.AddElement(new RPSCardsEngine.Cards.Card(CardType.Water)));


            var card3 = new RPSCardsEngine.Cards.Card(CardType.Rock, Zone.Hand, 0);
            Assert.ThrowsException<InvalidOperationException>(() => card3.AddElement(new RPSCardsEngine.Cards.Card(CardType.Water)));

            var card4 = new RPSCardsEngine.Cards.Card(CardType.Rock, Zone.Deck, 0);
            Assert.ThrowsException<InvalidOperationException>(() => card4.AddElement(new RPSCardsEngine.Cards.Card(CardType.Water)));

            var card5 = new RPSCardsEngine.Cards.Card(CardType.Rock, Zone.Graveyard, 0);
            Assert.ThrowsException<InvalidOperationException>(() => card5.AddElement(new RPSCardsEngine.Cards.Card(CardType.Water)));
        }
    }
}
