using UnityEngine;

namespace NewBuildingSystem
{
    public class CheckForBorders
    {
        private readonly GameObject _plane;
        
        private Color _notConstructed = new(1, 0, 0 , 0.39f);
        private Color _Constructed = new(1, 1, 1 , 0.39f);
        
        private bool _isBuild;

        public CheckForBorders(GameObject plane)
        {
            _plane = plane;
        }
        public bool IsPlaceTaken(Vector2 mousePos, Building flyingBuilding)
        {
            if (_isBuild && CheckPlaneForBorders(mousePos, flyingBuilding)) return true;
            
            return false;
        }
        
        private bool CheckPlaneForBorders(Vector2 mousePos, Building flyingBuilding)
        {
            var scale = _plane.transform.localScale;

            if (mousePos.x - TheRestOfTheSpaceMap().x < 0 ||
                mousePos.x - TheRestOfTheSpaceMap().x > scale.x * 10f - flyingBuilding.Size.x)
            {
                flyingBuilding.ChangePreviewColor(_notConstructed);
                return false;
            }

            if (mousePos.y - TheRestOfTheSpaceMap().y < 0 ||
                mousePos.y - TheRestOfTheSpaceMap().y > scale.z * 10f - flyingBuilding.Size.y)
            {
                flyingBuilding.ChangePreviewColor(_notConstructed);
                return false;
            }
            flyingBuilding.ChangePreviewColor(_Constructed);
            return true;
        }
        
        public void CheckPlaneForBuilding(bool isTriggered, Building flyingBuilding)
        {
            if (isTriggered)
            {
                flyingBuilding.ChangePreviewColor(_Constructed);
                _isBuild = true;
            }
            else
            {
                flyingBuilding.ChangePreviewColor(_notConstructed);
                _isBuild = false;
                
            }
            /*for (int x = 0; x < _flyingBuilding.size.x; x++)
            {
                for (int y = 0; y < _flyingBuilding.size.y; y++)
                {
                    if (_buildings[_mousePos.x + x, _mousePos.y + y] != null) return false;
                }
            }*/
            //return true;
        }

        private Vector2 TheRestOfTheSpaceMap()
        {
            var position = _plane.transform.position;
            var scale = _plane.transform.localScale;
            
            var MaPositionX = Mathf.RoundToInt(position.x - (scale.x * 10f) / 2f);
            var MaPositionY = Mathf.RoundToInt(position.z - (scale.z * 10f) / 2f);

            return new Vector2(MaPositionX, MaPositionY);
        }

    }
}