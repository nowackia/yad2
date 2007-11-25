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
using Yad.Board;

namespace Yad.Engine.Client {

    public delegate void BuildingChosenHandler(int objectID);
    public delegate void UnitChosenHandler(int id, string name);

    public interface IManageableStripe {
		event Yad.UI.Client.BuildStripe.ChoiceHandler OnChoice;
		void Add(int typeid, string name, String pictureName,bool building);
		void Remove(int id);
		void AddPercentCounter(int id);
		void SetPercentValue(int id,int val);
		void RemovePercentCounter(int id);
		void RemoveAll();
    }
}
