import React from 'react'
import { Outlet } from 'react-router-dom'
import Menu from './Menu'

export default function PrivateLayout(){
  return (
    <div>
      <nav className="navbar navbar-light bg-light d-flex d-md-none">
        <div className="container-fluid">
          <button className="btn btn-outline-secondary" type="button" data-bs-toggle="offcanvas" data-bs-target="#sidebar" aria-controls="sidebar">
            <i className="bi bi-list" />
          </button>
          <span className="navbar-brand ms-2">Painel</span>
        </div>
      </nav>

      <div className="container-fluid">
        <div className="row">
          <div className="col-12 col-md-3 col-lg-2 p-2">
            <div className="d-none d-md-block">
              <Menu />
            </div>
            <div className="offcanvas offcanvas-start d-md-none" tabIndex={-1} id="sidebar" aria-labelledby="sidebarLabel">
              <div className="offcanvas-header">
                <h5 className="offcanvas-title" id="sidebarLabel">Menu</h5>
                <button type="button" className="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>
              </div>
              <div className="offcanvas-body">
                <Menu />
              </div>
            </div>
          </div>
          <div className="col-12 col-md-9 col-lg-10 p-3">
            <div className="page-wrapper">
              <div className="page-card w-100">
                <Outlet />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
