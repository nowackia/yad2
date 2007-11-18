using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board.Common;
using Yad.Config.Common;
using Yad.Engine.Common;
using Yad.Config;
using System.IO;
using Yad.Properties;
using Yad.Properties.Client;

namespace Yad.Engine.Client {

    public delegate void OnBuildChosen(short id);
    public delegate void OnUnitChosen(short id);

    public interface IManageableStripe {
		event Yad.UI.Client.BuildStripe.ChoiceHandler OnChoice;
		void Add(short id,string name, String pictureName,bool building);
		void Remove(short id);
		void AddPercentCounter(short id);
		void SetPercentValue(short id,int val);
		void RemovePercentCounter(short id);
		void RemoveAll();
    }
}
