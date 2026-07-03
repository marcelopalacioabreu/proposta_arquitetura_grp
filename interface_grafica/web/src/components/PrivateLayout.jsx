import React from 'react'
import { Outlet } from 'react-router-dom'
import Menu from './Menu'
import Navbar from './Navbar'
import Footer from './Footer'

export default function PrivateLayout(){
  return (
    <div className="d-flex vh-100 w-100 page-root flex-column">
      <Navbar brand="Painel" />

      <div className="container-fluid flex-fill position-relative">
        {/* Floating Menu overlays the content; no reserved sidebar width */}
        <Menu />

        <div className="d-flex">
          {/* Mobile offcanvas retained for small screens */}
          <div className="d-md-none">
            <div className="offcanvas offcanvas-start" tabIndex={-1} id="sidebar" aria-labelledby="sidebarLabel">
              <div className="offcanvas-header">
                <h5 className="offcanvas-title" id="sidebarLabel">Menu</h5>
                <button type="button" className="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>
              </div>
              <div className="offcanvas-body">
                <Menu />
              </div>
            </div>
          </div>

          <div className="flex-fill p-3">
            <div className="page-wrapper">
              <div className="page-card w-100">
                <Outlet />
              </div>
            </div>
          </div>
        </div>
      </div>

      <Footer />
    </div>
  )
}
