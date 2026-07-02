import React from 'react'
import { Outlet } from 'react-router-dom'
import Menu from './Menu'

export default function PrivateLayout(){
  return (
    <div>
      <div className="header">Painel</div>
      <div className="container-fluid">
        <div className="row">
          <div className="col-12 col-md-3 col-lg-2 p-2">
            <Menu />
          </div>
          <div className="col-12 col-md-9 col-lg-10 p-3">
            <Outlet />
          </div>
        </div>
      </div>
    </div>
  )
}
