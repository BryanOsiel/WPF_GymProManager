-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost:3306
-- Tiempo de generación: 01-05-2024 a las 03:06:58
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `gym`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tasistencia`
--

CREATE TABLE `tasistencia` (
  `ID` int(11) NOT NULL,
  `TClienteID` int(11) NOT NULL,
  `FechaHoraAsistencia` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `tasistencia`
--

INSERT INTO `tasistencia` (`ID`, `TClienteID`, `FechaHoraAsistencia`) VALUES
(1, 1, '2024-04-28 22:25:47'),
(2, 1, '2024-04-29 15:57:07'),
(3, 2, '2024-04-29 19:34:35'),
(4, 1, '2024-04-30 01:59:02');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tclientes`
--

CREATE TABLE `tclientes` (
  `ID` int(11) NOT NULL,
  `TSucursalID` int(11) NOT NULL,
  `TDatosClientesID` int(11) NOT NULL,
  `MembresiaID` int(11) NOT NULL,
  `FechaRegistro` date NOT NULL,
  `Acceso` varchar(5) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `tclientes`
--

INSERT INTO `tclientes` (`ID`, `TSucursalID`, `TDatosClientesID`, `MembresiaID`, `FechaRegistro`, `Acceso`) VALUES
(1, 1, 1, 1, '2024-04-23', '12345'),
(2, 1, 2, 2, '2024-04-29', '45244');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tdatosclientes`
--

CREATE TABLE `tdatosclientes` (
  `ID` int(11) NOT NULL,
  `TDireccionClientesID` int(11) NOT NULL,
  `Nombre` varchar(20) NOT NULL,
  `ApellidoPaterno` varchar(20) NOT NULL,
  `ApellidoMaterno` varchar(45) NOT NULL,
  `Genero` varchar(15) NOT NULL,
  `FechaNacimiento` date NOT NULL,
  `CorreoElectronico` varchar(45) NOT NULL,
  `Telefono` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `tdatosclientes`
--

INSERT INTO `tdatosclientes` (`ID`, `TDireccionClientesID`, `Nombre`, `ApellidoPaterno`, `ApellidoMaterno`, `Genero`, `FechaNacimiento`, `CorreoElectronico`, `Telefono`) VALUES
(1, 1, 'Osiel', 'Casas', 'Lopez', 'masculino', '2001-05-11', 'osiel.casas@gmail.com', '3221989711'),
(2, 2, 'Paulina', 'Calvario', 'Pulido', 'Femenino', '2001-06-28', 'pauli@gmail.com', '3227791486');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tdatosempleados`
--

CREATE TABLE `tdatosempleados` (
  `ID` int(11) NOT NULL,
  `TDireccionEmpleadosID` int(11) NOT NULL,
  `Nombre` varchar(20) NOT NULL,
  `ApellidoPaterno` varchar(20) NOT NULL,
  `ApellidoMaterno` varchar(45) NOT NULL,
  `Genero` varchar(15) NOT NULL,
  `FechaNacimiento` date NOT NULL,
  `CorreoElectronico` varchar(45) NOT NULL,
  `Telefono` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `tdatosempleados`
--

INSERT INTO `tdatosempleados` (`ID`, `TDireccionEmpleadosID`, `Nombre`, `ApellidoPaterno`, `ApellidoMaterno`, `Genero`, `FechaNacimiento`, `CorreoElectronico`, `Telefono`) VALUES
(1, 1, 'Osiel', 'Casas', 'Lopez', 'Masculino', '2001-05-11', 'osiel.casas@gmail.com', '3221989711');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tdireccionclientes`
--

CREATE TABLE `tdireccionclientes` (
  `ID` int(11) NOT NULL,
  `Numero` int(11) NOT NULL,
  `Calle` varchar(25) NOT NULL,
  `Colonia` varchar(25) NOT NULL,
  `CodigoPostal` int(11) NOT NULL,
  `Municipio` varchar(45) NOT NULL,
  `Estado` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `tdireccionclientes`
--

INSERT INTO `tdireccionclientes` (`ID`, `Numero`, `Calle`, `Colonia`, `CodigoPostal`, `Municipio`, `Estado`) VALUES
(1, 207, 'Av. paseo de las sirenas', 'Pacifico Azul', 48280, 'Puerto Vallarta', 'Jalisco'),
(2, 413, 'Valle de montecarlo', 'Valle dorado', 63735, 'Bahia de banderas', 'Nayarit');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tdireccionempleados`
--

CREATE TABLE `tdireccionempleados` (
  `ID` int(11) NOT NULL,
  `Numero` int(11) NOT NULL,
  `Calle` varchar(25) NOT NULL,
  `Colonia` varchar(25) NOT NULL,
  `CodigoPostal` int(11) NOT NULL,
  `Municipio` varchar(45) NOT NULL,
  `Estado` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `tdireccionempleados`
--

INSERT INTO `tdireccionempleados` (`ID`, `Numero`, `Calle`, `Colonia`, `CodigoPostal`, `Municipio`, `Estado`) VALUES
(1, 207, 'Av. paseo de las sirenas', 'Pacifico Azul', 48280, 'Puerto Vallarta', 'Jalisco');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tdireccionproveedores`
--

CREATE TABLE `tdireccionproveedores` (
  `ID` int(11) NOT NULL,
  `Numero` int(11) NOT NULL,
  `Calle` varchar(25) NOT NULL,
  `Colonia` varchar(25) NOT NULL,
  `CodigoPostal` int(5) NOT NULL,
  `Municipio` varchar(45) NOT NULL,
  `Estado` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `tdireccionproveedores`
--

INSERT INTO `tdireccionproveedores` (`ID`, `Numero`, `Calle`, `Colonia`, `CodigoPostal`, `Municipio`, `Estado`) VALUES
(1, 1432, 'San Marino', 'Ixtapa', 48280, 'Puerto Vallarta', 'Jalisco');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tdireccionsucursales`
--

CREATE TABLE `tdireccionsucursales` (
  `ID` int(11) NOT NULL,
  `Numero` int(11) NOT NULL,
  `Calle` varchar(25) NOT NULL,
  `Colonia` varchar(25) NOT NULL,
  `CodigoPostal` int(5) NOT NULL,
  `Municipio` varchar(25) NOT NULL,
  `Estado` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `tdireccionsucursales`
--

INSERT INTO `tdireccionsucursales` (`ID`, `Numero`, `Calle`, `Colonia`, `CodigoPostal`, `Municipio`, `Estado`) VALUES
(1, 1432, 'San Marino', 'Ixtapa', 48280, 'Puerto Vallarta', 'Jalisco');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `templeados`
--

CREATE TABLE `templeados` (
  `ID` int(11) NOT NULL,
  `TSucursalID` int(11) NOT NULL,
  `TDatosEmpleadosID` int(11) NOT NULL,
  `Puesto` varchar(50) NOT NULL,
  `Turno` varchar(45) NOT NULL,
  `FechaContratacion` date NOT NULL,
  `Salario` decimal(10,2) NOT NULL,
  `Acceso` varchar(5) NOT NULL,
  `Contrasena` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `templeados`
--

INSERT INTO `templeados` (`ID`, `TSucursalID`, `TDatosEmpleadosID`, `Puesto`, `Turno`, `FechaContratacion`, `Salario`, `Acceso`, `Contrasena`) VALUES
(1, 1, 1, 'Administrador', 'Matutino', '2024-04-14', 5000.00, '12345', 'osiel');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tequipos`
--

CREATE TABLE `tequipos` (
  `ID` int(11) NOT NULL,
  `TSucursalID` int(11) NOT NULL,
  `NombreEquipo` varchar(50) NOT NULL,
  `Cantidad` int(3) NOT NULL,
  `Descripcion` varchar(100) NOT NULL,
  `Estado` varchar(20) NOT NULL,
  `Marca` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `tequipos`
--

INSERT INTO `tequipos` (`ID`, `TSucursalID`, `NombreEquipo`, `Cantidad`, `Descripcion`, `Estado`, `Marca`) VALUES
(1, 1, 'Caminadora electrica', 3, 'Caminadora profesional Centurfit MKZ-CAMW668 plegable.', 'Nuevo', 'Centurfit');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tmembresias`
--

CREATE TABLE `tmembresias` (
  `ID` int(11) NOT NULL,
  `TipoMembresia` varchar(50) NOT NULL,
  `DuracionMeses` int(2) NOT NULL,
  `Precio` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `tmembresias`
--

INSERT INTO `tmembresias` (`ID`, `TipoMembresia`, `DuracionMeses`, `Precio`) VALUES
(1, 'Mensual', 1, 450.00),
(2, 'Semestral', 6, 2600.00),
(3, 'Anual', 12, 5100.00);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tproductos`
--

CREATE TABLE `tproductos` (
  `ID` int(11) NOT NULL,
  `TProveedorID` int(11) NOT NULL,
  `NombreProducto` varchar(50) NOT NULL,
  `Precio` decimal(10,2) NOT NULL,
  `Stock` int(3) NOT NULL,
  `Descripcion` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `tproductos`
--

INSERT INTO `tproductos` (`ID`, `TProveedorID`, `NombreProducto`, `Precio`, `Stock`, `Descripcion`) VALUES
(1, 1, 'Agua Ciel', 20.00, 100, 'Agua Ciel 1L.'),
(2, 1, 'Gatorade', 25.00, 100, 'Bebida deportiva marca Gatorade sabor naranja 600 ml.'),
(3, 1, 'Toalla', 200.00, 200, 'Toallas Negras para Manos - 16 x 28\"');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tproveedores`
--

CREATE TABLE `tproveedores` (
  `ID` int(11) NOT NULL,
  `NombreProveedor` varchar(50) NOT NULL,
  `Telefono` varchar(10) NOT NULL,
  `CorreoElectronico` varchar(50) NOT NULL,
  `TDireccionProveedoresID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `tproveedores`
--

INSERT INTO `tproveedores` (`ID`, `NombreProveedor`, `Telefono`, `CorreoElectronico`, `TDireccionProveedoresID`) VALUES
(1, 'Osiel', '3221989711', 'osiel.casas@gmail.com', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tsucursales`
--

CREATE TABLE `tsucursales` (
  `ID` int(11) NOT NULL,
  `TDireccionSucursalesID` int(11) NOT NULL,
  `NombreSucursal` varchar(50) NOT NULL,
  `Telefono` varchar(10) NOT NULL,
  `CorreoElectronico` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `tsucursales`
--

INSERT INTO `tsucursales` (`ID`, `TDireccionSucursalesID`, `NombreSucursal`, `Telefono`, `CorreoElectronico`) VALUES
(1, 1, 'Arnold Gym', '3225687453', 'gymarnold@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tventaproducto`
--

CREATE TABLE `tventaproducto` (
  `ID` int(11) NOT NULL,
  `TProductoID` int(11) NOT NULL,
  `TSucursalID` int(11) NOT NULL,
  `Cantidad` int(11) NOT NULL,
  `MontoTotal` decimal(10,2) NOT NULL,
  `FechaVenta` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `tventaproducto`
--

INSERT INTO `tventaproducto` (`ID`, `TProductoID`, `TSucursalID`, `Cantidad`, `MontoTotal`, `FechaVenta`) VALUES
(1, 1, 1, 2, 40.00, '2024-04-30 01:55:03'),
(2, 1, 1, 7, 140.00, '2024-04-30 01:56:34');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `tasistencia`
--
ALTER TABLE `tasistencia`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `fk_TAsistencia_TClientes1_idx` (`TClienteID`);

--
-- Indices de la tabla `tclientes`
--
ALTER TABLE `tclientes`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `fk_TClientes_TDatosClientes1_idx` (`TDatosClientesID`),
  ADD KEY `fk_TClientes_TSucursales1_idx` (`TSucursalID`),
  ADD KEY `fk_TClientes_TMembresias_idx` (`MembresiaID`);

--
-- Indices de la tabla `tdatosclientes`
--
ALTER TABLE `tdatosclientes`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `fk_TDatosClientes_TDireccionClientes1_idx` (`TDireccionClientesID`);

--
-- Indices de la tabla `tdatosempleados`
--
ALTER TABLE `tdatosempleados`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `fk_TDatosEmpleados_TDireccionEmpleados1_idx` (`TDireccionEmpleadosID`);

--
-- Indices de la tabla `tdireccionclientes`
--
ALTER TABLE `tdireccionclientes`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `tdireccionempleados`
--
ALTER TABLE `tdireccionempleados`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `tdireccionproveedores`
--
ALTER TABLE `tdireccionproveedores`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `tdireccionsucursales`
--
ALTER TABLE `tdireccionsucursales`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `templeados`
--
ALTER TABLE `templeados`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `fk_TEmpleados_TDatosEmpleados1_idx` (`TDatosEmpleadosID`),
  ADD KEY `fk_TEmpleados_TSucursales1_idx` (`TSucursalID`);

--
-- Indices de la tabla `tequipos`
--
ALTER TABLE `tequipos`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `fk_TEquipos_TSucursales1_idx` (`TSucursalID`);

--
-- Indices de la tabla `tmembresias`
--
ALTER TABLE `tmembresias`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `tproductos`
--
ALTER TABLE `tproductos`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `fk_TProducto_TProveedor1_idx` (`TProveedorID`);

--
-- Indices de la tabla `tproveedores`
--
ALTER TABLE `tproveedores`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `fk_TProveedor_TDireccionProveedores1_idx` (`TDireccionProveedoresID`);

--
-- Indices de la tabla `tsucursales`
--
ALTER TABLE `tsucursales`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `fk_TSucursales_TDireccionSucursales1_idx` (`TDireccionSucursalesID`);

--
-- Indices de la tabla `tventaproducto`
--
ALTER TABLE `tventaproducto`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `fk_TVentaProducto_TProducto1_idx` (`TProductoID`),
  ADD KEY `fk_TVentaProducto_TSucursales1_idx` (`TSucursalID`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `tasistencia`
--
ALTER TABLE `tasistencia`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `tclientes`
--
ALTER TABLE `tclientes`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `tdatosclientes`
--
ALTER TABLE `tdatosclientes`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `tdatosempleados`
--
ALTER TABLE `tdatosempleados`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `tdireccionclientes`
--
ALTER TABLE `tdireccionclientes`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `tdireccionempleados`
--
ALTER TABLE `tdireccionempleados`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `tdireccionproveedores`
--
ALTER TABLE `tdireccionproveedores`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `tdireccionsucursales`
--
ALTER TABLE `tdireccionsucursales`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `templeados`
--
ALTER TABLE `templeados`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `tequipos`
--
ALTER TABLE `tequipos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `tmembresias`
--
ALTER TABLE `tmembresias`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `tproductos`
--
ALTER TABLE `tproductos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `tproveedores`
--
ALTER TABLE `tproveedores`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `tsucursales`
--
ALTER TABLE `tsucursales`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `tventaproducto`
--
ALTER TABLE `tventaproducto`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `tasistencia`
--
ALTER TABLE `tasistencia`
  ADD CONSTRAINT `fk_TAsistencia_TClientes1` FOREIGN KEY (`TClienteID`) REFERENCES `tclientes` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `tclientes`
--
ALTER TABLE `tclientes`
  ADD CONSTRAINT `fk_TClientes_TDatosClientes1` FOREIGN KEY (`TDatosClientesID`) REFERENCES `tdatosclientes` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_TClientes_TMembresias` FOREIGN KEY (`MembresiaID`) REFERENCES `tmembresias` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_TClientes_TSucursales1` FOREIGN KEY (`TSucursalID`) REFERENCES `tsucursales` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `tdatosclientes`
--
ALTER TABLE `tdatosclientes`
  ADD CONSTRAINT `fk_TDatosClientes_TDireccionClientes1` FOREIGN KEY (`TDireccionClientesID`) REFERENCES `tdireccionclientes` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `tdatosempleados`
--
ALTER TABLE `tdatosempleados`
  ADD CONSTRAINT `fk_TDatosEmpleados_TDireccionEmpleados1` FOREIGN KEY (`TDireccionEmpleadosID`) REFERENCES `tdireccionempleados` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `templeados`
--
ALTER TABLE `templeados`
  ADD CONSTRAINT `fk_TEmpleados_TDatosEmpleados1` FOREIGN KEY (`TDatosEmpleadosID`) REFERENCES `tdatosempleados` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_TEmpleados_TSucursales1` FOREIGN KEY (`TSucursalID`) REFERENCES `tsucursales` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `tequipos`
--
ALTER TABLE `tequipos`
  ADD CONSTRAINT `fk_TEquipos_TSucursales1` FOREIGN KEY (`TSucursalID`) REFERENCES `tsucursales` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `tproductos`
--
ALTER TABLE `tproductos`
  ADD CONSTRAINT `fk_TProducto_TProveedor1` FOREIGN KEY (`TProveedorID`) REFERENCES `tproveedores` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `tproveedores`
--
ALTER TABLE `tproveedores`
  ADD CONSTRAINT `fk_TProveedor_TDireccionProveedores1` FOREIGN KEY (`TDireccionProveedoresID`) REFERENCES `tdireccionproveedores` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `tsucursales`
--
ALTER TABLE `tsucursales`
  ADD CONSTRAINT `fk_TSucursales_TDireccionSucursales1` FOREIGN KEY (`TDireccionSucursalesID`) REFERENCES `tdireccionsucursales` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `tventaproducto`
--
ALTER TABLE `tventaproducto`
  ADD CONSTRAINT `fk_TVentaProducto_TProducto1` FOREIGN KEY (`TProductoID`) REFERENCES `tproductos` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_TVentaProducto_TSucursales1` FOREIGN KEY (`TSucursalID`) REFERENCES `tsucursales` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
