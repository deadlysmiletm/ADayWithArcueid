/** GPL v3 License
 * 
 * Copyright © 2019 DeadlySmile
 * Copyright © 2019 Lucas Leonardo Conti
 * 
 * GameEventListener.cs is part of ParadoxFramework.
 * 
 * ParadoxFramework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * ParadoxFramework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with ParadoxFramework. If not, see <http://www.gnu.org/licenses/>.
 **/

using UnityEngine;
using UnityEngine.Events;

namespace ParadoxFramework.Events
{
    public class GameEventListener : MonoBehaviour
    {
        /// <summary>
        /// GameEvent to suscribe.
        /// </summary>
        public GameEvent serializedEvent;
        /// <summary>
        /// Events registed in this listener.
        /// </summary>
        public UnityEvent OnExecute;


        public virtual void OnAwake() => serializedEvent.SuscribeListener(this);
        public virtual void OnSleep() => serializedEvent.UnsuscribeListener(this);


        /// <summary>
        /// Execute the response event.
        /// </summary>
        public virtual void OnExecuteEvent() => OnExecute.Invoke();


        private void OnEnable() => OnAwake();
        private void OnDisable() => OnSleep();
    }
}