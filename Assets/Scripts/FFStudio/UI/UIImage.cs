using UnityEngine;
using UnityEngine.UI;

namespace FFStudio
{
	public class UIImage : UIEntity
	{
		[SerializeField]
		private Image imageRenderer;
		Vector2 imageSizeDelta;
		private void Awake()
		{
			imageSizeDelta = uiTransform.sizeDelta;
		}

		public void SetSprite( Sprite sprite )
		{
			// If ImageRenderer object size is bigger then sprite
			if( imageSizeDelta.x > sprite.textureRect.width && imageSizeDelta.y > sprite.textureRect.height )
			{
				imageRenderer.preserveAspect = false;
				uiTransform.sizeDelta = new Vector2( sprite.textureRect.width, sprite.textureRect.height );
			}
			else // If sprite's any side is bigger then ImageRenderer object 
			{
				uiTransform.sizeDelta = imageSizeDelta;
				imageRenderer.preserveAspect = true;
			}

			imageRenderer.sprite = sprite;
		}
	}
}