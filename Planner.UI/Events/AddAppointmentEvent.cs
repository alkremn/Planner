﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Planner.UI.Events
{
    public class AddAppointmentEvent: PubSubEvent<int>
    {
    }
}
