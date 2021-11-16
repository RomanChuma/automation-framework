using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Enums;
using AutomationFramework.Core.Utils;
using AutomationFramework.Core.Utils.Log;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls
{
	/// <summary>
	/// Concrete implementation of radiobutton element
	/// </summary>
	public class InputRadioButtonElement : UiElement, IRadioButton
	{
		private readonly IWebElement _radioButton;
		private readonly ILogger _log = Log4NetLogger.Instance;

		public InputRadioButtonElement(IWebElement webElement) : base(webElement)
		{
			_radioButton = webElement;
		}

		public bool IsSelected => _radioButton.Selected;

		/// <summary>
		/// Is radiobutton enabled
		/// </summary>
		public bool IsEnabled => _radioButton.Enabled;

		/// <summary>
		/// Select the radiobutton element
		/// </summary>
		/// <param name="actionType"></param>
		public void Select(ActionType actionType = ActionType.Default)
		{
			_log.Trace("Selecting the radiobutton");

			if (IsEnabled)
			{
				if (IsSelected)
				{
					_log.Trace("Radiobutton is already selected, skipping...");
				}
				else
				{
					switch (actionType)
					{
						case ActionType.Default:
						{
							Click();
							break;
						}

						case ActionType.Mouse:
						{
							Click(ActionType.Mouse);
							break;
						}

						case ActionType.JavaScript:
							Click(ActionType.JavaScript);

							Wait.ForPageReadyStateToComplete();
							Wait.ForAngularToComplete();
							break;
					}

					RemoveFocus();
				}
			}
			else
			{
				_log.Warn("Can not select the disabled radiobutton");
			}
		}
	}
}
