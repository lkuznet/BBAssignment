using CatalogManager.Domain.Aggregates;
using CatalogManager.Infractructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Application.Catalog
{
    public class CatalogApiWithLogging : ICatalogApi
    {
        ICatalogApi _decoratedComponent;
        private ILog _errorLog;

        public CatalogApiWithLogging(ICatalogApi decoratedComponent, ILog errorLog)
        {
            _decoratedComponent = decoratedComponent;
            _errorLog = errorLog;
        }
        public bool AddCategory(int parentId, CategoryDto categoryDto)
        {
            try
            {
                return _decoratedComponent.AddCategory(parentId, categoryDto);
            }
            catch (Exception ex)
            {
                _errorLog.LogError("It's been an error, method: AddCategory. Error:" + ex.Message + ex.StackTrace);
            }
            return false;
        }

        public bool AddProduct(int parentId, ProductDto productDto)
        {
            try
            {
                return _decoratedComponent.AddProduct(parentId, productDto);
            }
            catch (Exception ex)
            {
                _errorLog.LogError("It's been an error, method: AddProduct. Error:" + ex.Message + ex.StackTrace);
            }
            return false;
        }

        public string GetCatalog(int indent)
        {
            throw new NotImplementedException();
        }

        public List<Domain.CatalogItemDto> GetCatalogTree(int indent)
        {
            try
            {
                return _decoratedComponent.GetCatalogTree(indent);
            }
            catch (Exception ex)
            {
                _errorLog.LogError("It's been an error, method: GetCatalogTree. Error:" + ex.Message + ex.StackTrace);
            }
            return null;
        }

        public ProductDto GetProduct(int id)
        {
            try
            {
                return _decoratedComponent.GetProduct(id);
            }
            catch (Exception ex)
            {
                _errorLog.LogError("It's been an error, method: GetProduct. Error:" + ex.Message + ex.StackTrace);
            }
            return null;
        }

        public IEnumerable<ICatalogItem> GetProducts(int parentId)
        {
            try
            {
                return _decoratedComponent.GetProducts(parentId);
            }
            catch (Exception ex)
            {
                _errorLog.LogError("It's been an error, method: GetProducts. Error:" + ex.Message + ex.StackTrace);
            }
            return null;
        }

        public bool RemoveCategory(int childId)
        {
            try
            {
                return _decoratedComponent.RemoveCategory(childId);
            }
            catch(Exception ex)
            {
                _errorLog.LogError("It's been an error, method: RemoveCategory. Error:" + ex.Message + ex.StackTrace);
            }
            return false;
        }

        public bool RemoveProduct(int parentId, int id)
        {
            try
            {
                return _decoratedComponent.RemoveProduct(parentId, id);
            }
            catch (Exception ex)
            {
                _errorLog.LogError("It's been an error, method: RemoveProduct. Error:" + ex.Message + ex.StackTrace);
            }
            return false;
        }

        public bool UpdateCategory(int childId, CategoryDto categoryDto)
        {
            try
            {
                return _decoratedComponent.UpdateCategory(childId, categoryDto);
            }
            catch (Exception ex)
            {
                _errorLog.LogError("It's been an error, method: UpdateCategory. Error:" + ex.Message + ex.StackTrace);
            }
            return false;
        }

        public bool UpdateProduct(int parentId, int childId, ProductDto productDto)
        {
            try
            {
                return _decoratedComponent.UpdateProduct(parentId, childId, productDto);
            }
            catch (Exception ex)
            {
                _errorLog.LogError("It's been an error, method: UpdateProduct. Error:" + ex.Message + ex.StackTrace);
            }
            return false;
        }
    }


}
