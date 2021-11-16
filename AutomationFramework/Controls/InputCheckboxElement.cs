using System.Collections.Generic;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Enums;
using AutomationFramework.Core.Utils;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls
{
	/// <summary>
	/// Concrete implementation of checkbox element
	/// </summary>
	public class InputCheckboxElement : UiElement, ICheckbox
	{
		private readonly IWebElement _checkbox;

		public InputCheckboxElement(IWebElement webElement)
			: base(webElement)
		{
			_checkbox = webElement;
		}

		public bool IsChecked => _checkbox.Selected;

		/// <summary>
		/// Is element enabled
		/// </summary>
		public bool IsEnabled => _checkbox.Enabled;

		public List<ElementState> State
		{
			get
			{
				var checkboxStates = new List<ElementState>();

				if (IsEnabled)
				{
					checkboxStates.Add(ElementState.Enabled);
				}
				else
				{
					checkboxStates.Add(ElementState.Disabled);
				}

				if (IsChecked)
				{
					checkboxStates.Add(ElementState.Checked);
				}
				else
				{
					checkboxStates.Add(ElementState.Unchecked);
				}

				return checkboxStates;
			}
		}

		public void Check(ActionType actionType = ActionType.Default)
		{
			Log.Trace("Checking out the checkbox");

			if (IsEnabled)
			{
				if (IsChecked)
				{
					Log.Trace("Checkbox is already checked, skipping...");
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
							Browser.InvokeScript("arguments[0].checked = true;", _checkbox);

							Wait.ForPageReadyStateToComplete();
							Wait.ForAngularToComplete();

							break;
					}
				}
			}
			else
			{
				Log.Warn("Can not check the disabled checkbox");
			}
		}

		public void Uncheck(ActionType actionType = ActionType.Default)
		{
			Log.Trace("Unchecking out the checkbox");

			if (IsEnabled)
			{
				if (!IsChecked)
				{
					Log.Trace("Checkbox is already unchecked, skipping...");
				}
				else
				{
					switch (actionType)
					{
						case ActionType.Default:
							{
								Click();
								RemoveFocus();
								break;
							}

						case ActionType.Mouse:
							{
								Click(ActionType.Mouse);
								RemoveFocus();
								break;
							}

						case ActionType.JavaScript:
							Browser.InvokeScript("arguments[0].checked = false;", _checkbox);
							RemoveFocus();

							Wait.ForPageReadyStateToComplete();
							Wait.ForAngularToComplete();

							break;
					}
				}
			}
			else
			{
				Log.Warn("Can not uncheck the disabled checkbox");
			}
		}
	}
}