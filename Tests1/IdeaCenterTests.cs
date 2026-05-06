using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using IdeaCenterPom.Pages;

namespace IdeaCenterPom.Tests
{
    public class IdeaCenterTests : BaseTest
    {
        private static string lastCreatedIdeaTitle;
        private static string lastCreatedIdeaDescription;

        [Test, Order(1)]
        public void CreateIdeaWithInvalidDataTest()
        {
            createIdeaPage.OpenPage();

            createIdeaPage.CreateIdea("", "", "");

            createIdeaPage.AssertErrorMessages();
        }

        [Test, Order(2)]
        public void CreateRandomIdeaTest()
        {
            lastCreatedIdeaTitle = "Idea " + GenerateRandomString(5);
            lastCreatedIdeaDescription = "Description " + GenerateRandomString(10);

            createIdeaPage.OpenPage();

            createIdeaPage.CreateIdea(lastCreatedIdeaTitle, "http://www.pictures.com/picture.jpg", lastCreatedIdeaDescription);

            Assert.That(driver.Url, Is.EqualTo(myIdeasPage.Url), "The URL after creation did not match the expected URL.");

            Assert.That(myIdeasPage.IdeaDescriptionLastCard.Text.Trim(), Is.EqualTo(lastCreatedIdeaDescription), "The new idea was not found or is incorrectly listed in 'My Ideas'.");
        }

        [Test, Order(3)]
        public void ViewLastCreatedIdeaTest()
        {
            myIdeasPage.OpenPage();

            Assert.That(myIdeasPage.IdeaCards.Count, Is.GreaterThan(0), "No idea cards were found on the page.");

            myIdeasPage.ViewButtonLastCard.Click();

            Assert.That(ideasReadPage.IdeaTitle.Text.Trim(), Is.EqualTo(lastCreatedIdeaTitle), "The title of the idea does not match the expected value.");

        }

        [Test, Order(4)]
        public void EditLastCreatedIdeaTitleTest()
        {
            myIdeasPage.OpenPage();

            Assert.IsTrue(myIdeasPage.IdeaCards.Count > 0, "No idea cards were found on the page.");

            myIdeasPage.EditButtonLastCard.Click();

            string newTitle = "Changed Title: " + lastCreatedIdeaTitle;
            ideasEditPage.IdeaTitle.Clear();
            ideasEditPage.IdeaTitle.SendKeys(newTitle);
            ideasEditPage.EditButton.Click();

            myIdeasPage.OpenPage();

            myIdeasPage.ViewButtonLastCard.Click();

            Assert.That(ideasReadPage.IdeaTitle.Text.Trim(), Is.EqualTo(newTitle), "The title of the idea does not match the expected value.");
        }

        [Test, Order(5)]
        public void EditIdeaDescriptionTest()
        {
            myIdeasPage.OpenPage();

            Assert.IsTrue(myIdeasPage.IdeaCards.Count > 0, "No idea cards were found on the page.");

            myIdeasPage.EditButtonLastCard.Click();

            string newDescription = "Changed Description: " + lastCreatedIdeaDescription;

            ideasEditPage.IdeaDescription.Clear();
            ideasEditPage.IdeaDescription.SendKeys(newDescription);
            ideasEditPage.EditButton.Click();

            Assert.True(driver.Url.Contains(myIdeasPage.Url), "The URL after editing did not match the expected URL.");

            myIdeasPage.ViewButtonLastCard.Click();

            Assert.That(ideasReadPage.IdeaDescription.Text.Trim(), Is.EqualTo(newDescription), "The description of the idea did not update as expected.");
        }

        [Test, Order(6)]
        public void DeleteLastIdeaTest()
        {
            myIdeasPage.OpenPage();

            Assert.IsTrue(myIdeasPage.IdeaCards.Count > 0, "No idea cards were found on the page.");

            myIdeasPage.DeleteButtonLastCard.Click();

            bool isIdeaDeleted = myIdeasPage.IdeaCards.All(card => !card.Text.Contains(lastCreatedIdeaDescription));

            Assert.IsTrue(isIdeaDeleted, "The idea was not deleted successfully or is still visible in the list.");
        }
    }
}