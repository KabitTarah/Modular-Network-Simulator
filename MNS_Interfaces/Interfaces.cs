using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_Reporting;
using Location;

namespace ModularNetworkSimulator
{
    // The Observer interfaces are moving to the MNS_Reporting Namespace
    public interface IObserver
    { // Observer pattern. The View must register as an observer. Reports will be sent to the View 
      // at UpdateFrequency intervals or when changes occur (for static Reports).
      // NOTE: this interface is more specific than in the typical Observer pattern.
        double UpdateFrequency { get; }

        void Notify(IReport report);
    }

    public interface IObservable
    { // Observer pattern. This is the Model side. The View registers with the model here.
        void Register(IObserver observer);
        void UnRegister(IObserver observer);
    }
}
