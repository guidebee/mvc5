using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {


        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ ProductId = 1,Name="P1",Category = "Apples"},
                new Product{ ProductId = 2,Name="P2",Category = "Apples"},
                new Product{ ProductId = 3,Name="P3",Category = "Plums"},
                new Product{ ProductId = 4,Name="P4",Category = "Oranges"},
      
            });

            //Arrange - create the controller
            NavController target=new NavController(mock.Object);

            //Act =get the set of categories
            string[] results = ((IOrderedEnumerable<string>) target.Menu().Model).ToArray();

            //Assert
            Assert.AreEqual(results.Length,3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ ProductId = 1,Name="P1",Category = "Cat1"},
                new Product{ ProductId = 2,Name="P2",Category = "Cat2"},
                new Product{ ProductId = 3,Name="P3",Category = "Cat1"},
                new Product{ ProductId = 4,Name="P4",Category = "Cat2"},
                new Product{ ProductId = 5,Name="P5",Category = "Cat3"}
            });

            ProductController controller = new ProductController(mock.Object);

            //Action
            var result = ((ProductsListViewModel)controller.List("Cat2").Model).Products.ToArray();

            //Assert
           
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
            
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ ProductId = 1,Name="P1"},
                new Product{ ProductId = 2,Name="P2"},
                new Product{ ProductId = 3,Name="P3"},
                new Product{ ProductId = 4,Name="P4"},
                new Product{ ProductId = 5,Name="P5"}
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act
            ProductsListViewModel result = (ProductsListViewModel) controller.List(null,2).Model;

            //Assert
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage,2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);

        }

        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductsRepository> mock=new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ ProductId = 1,Name="P1"},
                new Product{ ProductId = 2,Name="P2"},
                new Product{ ProductId = 3,Name="P3"},
                new Product{ ProductId = 4,Name="P4"},
                new Product{ ProductId = 5,Name="P5"}
            });

            ProductController controller=new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;

            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length==2);
            Assert.AreEqual(prodArray[0].Name,"P4");
            Assert.AreEqual(prodArray[1].Name,"P5");

        }


        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Arrange - define an HTML helper - we need to this
            //in order to apply the extension method

            HtmlHelper myHelper = null;
            
            //Arrange - create PageInfo data
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            //Arrange - set up the delegate using a lambda expression

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //Act

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //Assert
            Assert.AreEqual(result.ToString(), @"<a class=""btn btn-default"" href=""Page1"">1</a><a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>");

        }
    }
}
